unit Map.Frame.InteractiveMap;

interface

uses
  System.SysUtils, System.Types, System.UITypes, System.Classes, System.Variants,
  Generics.Collections, FMX.Types, FMX.Graphics, FMX.Controls, FMX.Forms, FMX.Dialogs,
  FMX.StdCtrls, FMX.Objects, FMX.Layouts, TM.Form.Wrapper, FMX.Controls.Presentation,
  System.ImageList, FMX.ImgList, TM.Map.Wrapper, ME.MarkerFilter, Map.Data.Types,
  Map.Frame.Marker, ME.DB.Map, ME.DB.Marker, ME.DB.Quest;

type
  TInteractiveMap = class(TFrame)
    MainContainer2: TScrollBox;
    Background: TImage;
    MainContainer: TLayout;
    MapTagImages: TImageList;
    PositionImage: TImage;
    MarkerPanel: TPanel;

    procedure BackgroundMouseDown(Sender: TObject; Button: TMouseButton; Shift: TShiftState; X, Y: Single);
    procedure BackgroundMouseMove(Sender: TObject; Shift: TShiftState; X, Y: Single);
    procedure BackgroundMouseUp(Sender: TObject; Button: TMouseButton; Shift: TShiftState; X, Y: Single);
    procedure BackgroundDblClick(Sender: TObject);
    procedure BackgroundMouseLeave(Sender: TObject);
  private
    FMapWrapper: TMapWrapper;
    FMousePosition: TMousePosition;
    FItems: TList<TImage>;
    FCurrent: TImage;
    FMarkerDescript: TMarkerDescript;
    FOnDoubleClick: TNotifyEvent;
    FOnMouseDown: TNotifyEvent;

  {$IFDEF DEBUG}
    FTestData: array of string;
    FTestIndex: Integer;
  {$ENDIF}

    function GetBitmap: TBitmap;
    procedure SetBitmap(const Value: TBitmap);
    procedure OnMapChange(Bitmap: TBitmap);
    function GetMarkerFilter: TMarkerFilter;
    function GetMap: TDBMap;
    procedure SetMap(const Value: TDBMap);

    procedure AddMarker(const Marker: TDBMarker; const Title: string; Trader: TTrader);
    procedure AddPosition(const Position: TPoint);
    procedure OnMarkerClick(Sender: TObject);
    procedure OnMarkerDescriptionClose(Sender: TObject);
    procedure OnQuestComplete(const Value: Boolean);
    function GetMarkerInfoVisible: Boolean;
  public
    constructor Create(AOwner: TComponent); override;
    destructor Destroy; override;

    procedure Center;
    procedure ZoomIn;
    procedure ZoomOut;
    procedure SetMouseDown(const Value: Boolean);
    procedure HideMarkerInfo;
    function NormalizePosition(const Left, Top: Integer):  TPoint;
    procedure Refresh;

  {$IFDEF DEBUG}
    procedure TestPosition;
  {$ENDIF}

    property Map: TDBMap read GetMap write SetMap;
    property Bitmap: TBitmap read GetBitmap write SetBitmap;
    property MarkerFilter: TMarkerFilter read GetMarkerFilter;
    property MarkerInfoVisible: Boolean read GetMarkerInfoVisible;
    property OnDoubleClick: TNotifyEvent read FOnDoubleClick write FOnDoubleClick;
    property OnMouseDown: TNotifyEvent read FOnMouseDown write FOnMouseDown;
  end;

implementation

uses
  App.Service, ME.Service.Marker;

const
  TransitExtractionImageIdx = 5;

{$R *.fmx}

{ TInteractiveMap }

constructor TInteractiveMap.Create(AOwner: TComponent);
begin
  inherited;

  Background.Position.X := 0;
  Background.Position.Y := 0;
  FOnDoubleClick := nil;
  FOnMouseDown := nil;

  FMapWrapper := TMapWrapper.Create(AppService.Options.SreenshotPath);
  FMapWrapper.TrackLocation := AppService.Options.TrackLocation;
  FMapWrapper.Images := MapTagImages;
  FMapWrapper.OnMapChange := OnMapChange;

  FMarkerDescript := TMarkerDescript.Create(Self);
  FMarkerDescript.Parent := MarkerPanel;
  FMarkerDescript.Align := TAlignLayout.Client;
  FMarkerDescript.OnClose := OnMarkerDescriptionClose;
  FMarkerDescript.OnQuestComplete := OnQuestComplete;

  FItems := TObjectList<TImage>.Create;
  FCurrent := nil;

  PositionImage.Visible := False;
  MarkerPanel.Visible := False;
  MarkerPanel.Height := 400;
  MarkerPanel.Width := 560;

{$IFDEF DEBUG}
  SetLength(FTestData, 9);
  FTestData[0] := '2024-07-06[22-01]_0.0, 0.0, 0.0_0.0, 0.0, 0.0, 0.0_0.00 (0).png';
  FTestData[1] := '2024-07-05[13-20]_234.2, -9.3, -6.7_0.0, 0.8, 0.0, -0.6_15.40 (0).png';
  FTestData[2] := '2024-07-05[13-22]_186.7, -3.4, 20.5_-0.1, 0.8, -0.1, -0.6_15.62 (0).png';
  FTestData[3] := '2024-07-05[13-23]_88.8, -5.5, -14.1_0.1, -0.7, 0.1, 0.8_15.79 (0).png';
  FTestData[4] := '2024-07-05[13-26]_-13.2, 19.9, 203.3_0.0, -1.0, 0.1, 0.3_16.07 (0).png';
  FTestData[5] := '2024-07-08[21-18]_0.0, 0.0, -100.0_0.0, 0.0, 0.0, 0.0_0.00 (0).png';
  FTestData[6] := '2024-07-08[21-19]_0.0, 0.0, 100.0_0.0, 0.0, 0.0, 0.0_0.00 (0).png';
  FTestData[7] := '2024-07-08[21-19]_100.0, 0.0, 0.0_0.0, 0.0, 0.0, 0.0_0.00 (0).png';
  FTestData[8] := '2024-07-11[18-53]_-162.0, 0.0, -115.0_0.0, 0.0, 0.0, 0.0_0.00 (0).png';

  FTestIndex := -1;
{$ENDIF}
end;

destructor TInteractiveMap.Destroy;
begin
  FOnDoubleClick := nil;
  FOnMouseDown := nil;
  FMapWrapper.Free;
  FItems.Free;

  inherited;
end;

function TInteractiveMap.GetBitmap: TBitmap;
begin
  Result := Background.Bitmap;
end;

procedure TInteractiveMap.SetBitmap(const Value: TBitmap);
begin
  Background.Width := Value.Width;
  Background.Height := Value.Height;
  Background.Bitmap.Assign(nil);
  Background.Bitmap.Assign(Value);
  Background.Visible := not Value.IsEmpty;
end;

procedure TInteractiveMap.OnMapChange(Bitmap: TBitmap);
var
  Marker: TDBMarker;
  Quest: TDBQuest;
  i: Integer;
begin
//{$IFNDEF DEBUG}
//  Logger.Lines.Add('OnMapChange');
//{$ENDIF}

  Self.Bitmap := Bitmap;

  FItems.Clear;
  for Marker in Map.Markers do
    if MarkerFilter.IsGropupEnable(Marker.Kind) then
      AddMarker(Marker, Marker.Caption, TTrader.None);

  for i := 0 to Map.Quests.Count - 1 do begin
    Quest := Map.Quests[i];

    if MarkerFilter.IsQuestEnable(i) then
      for Marker in Quest.Markers do
        AddMarker(Marker, Quest.Name, Quest.Trader);
  end;

  AddPosition(FMapWrapper.Position);
end;

function TInteractiveMap.GetMarkerFilter: TMarkerFilter;
begin
  Result := FMapWrapper.MarkerFilter;
end;

function TInteractiveMap.GetMarkerInfoVisible: Boolean;
begin
  Result := MarkerPanel.Visible;
end;

function TInteractiveMap.GetMap: TDBMap;
begin
  Result := FMapWrapper.Map;
end;

procedure TInteractiveMap.SetMap(const Value: TDBMap);
begin
  if (Value = nil) or (FMapWrapper.Map = Value) then
    Exit;

  FCurrent := nil;
  FMapWrapper.LoadMap(Value);
  FMapWrapper.Start;
end;

procedure TInteractiveMap.AddMarker(const Marker: TDBMarker; const Title: string; Trader: TTrader);
const
  MarkerHeight = 32;
  MarkerWidth = 32;
var
  Item: TImage;
//  Offset: Double;
  p: TPoint;
  ImageIdex: Integer;
begin
  p := NormalizePosition(Marker.Left, Marker.Top);
  p.Top := p.Top - MarkerHeight div 2;
  p.Left := p.Left - MarkerWidth div 2;

  Item := TImage.Create(Self);
  try
    Item.Height := MarkerHeight;
    Item.Width := MarkerWidth;
    Item.Parent := Background;
    Item.Position.X := p.Left;
    Item.Position.Y := p.Top;

    ImageIdex := Ord(Marker.Kind);
    if (Marker.Kind = TMarkerKind.Quest) and AppService.Profile.IsQuestPartFinished(Marker.ID) then
      ImageIdex := TransitExtractionImageIdx;

    Item.Bitmap.Assign(MapTagImages.Bitmap(TSizeF.Create(32, 32), ImageIdex));
    Item.Hint := Title;
    Item.ShowHint := Trim(Title) <> '';
    Item.OnClick := OnMarkerClick;
    Item.TagObject := Marker;
    Item.Tag := Ord(Trader);

    if (Marker.Images.Count > 0) or (Marker.Items.Count > 0) then
      Item.Cursor := crHandPoint
    else
      Item.Cursor := crDefault;
  finally
    FItems.Add(Item);
  end;
end;

procedure TInteractiveMap.AddPosition(const Position: TPoint);
const
  MarkerHeight = 16;
  MarkerWidth = 16;
var
  p: TPoint;
begin
  p := NormalizePosition(Position.Left, Position.Top);
  p.Top := p.Top - MarkerHeight div 2;
  p.Left := p.Left - MarkerWidth div 2;

  PositionImage.Position.X := p.Left;
  PositionImage.Position.Y := p.Top;
  PositionImage.Visible := not Position.Empty;
  PositionImage.BringToFront;
end;

procedure TInteractiveMap.OnMarkerClick(Sender: TObject);
var
  Marker: TDBMarker;
begin
  if Assigned(FOnMouseDown) then
    FOnMouseDown(Self);

  FCurrent := TImage(Sender);
  Marker := TDBMarker(FCurrent.TagObject);
//  MarkerService.LoadPictures(Marker.ID, Marker.Images);

  if (Marker.Images.Count > 0) or (Marker.Items.Count > 0) then begin
//  if (Marker.Images.Count > 0) and (Trim(Marker.Images[0].Description) <> '') then begin
    FMarkerDescript.Init(Marker, FCurrent.Hint, TTrader(FCurrent.Tag));

    MarkerPanel.Height := FMarkerDescript.MaxHeight;
    MarkerPanel.Width := FMarkerDescript.MaxWidth;
    MarkerPanel.Position.X := (MainContainer.Width - MarkerPanel.Width) / 2;
    MarkerPanel.Position.Y := (MainContainer.Height - MarkerPanel.Height) / 2;
    MarkerPanel.Visible := True;
  end;
end;

procedure TInteractiveMap.OnMarkerDescriptionClose(Sender: TObject);
begin
  MarkerPanel.Visible := False;
end;

procedure TInteractiveMap.OnQuestComplete(const Value: Boolean);
var
  Marker: TDBMarker;
  ImageIdex: Integer;
begin
  Marker := TDBMarker(FCurrent.TagObject);
  ImageIdex := Ord(Marker.Kind);
  if (Marker.Kind = TMarkerKind.Quest) and Value then
    ImageIdex := TransitExtractionImageIdx;

  FCurrent.Bitmap.Assign(MapTagImages.Bitmap(TSizeF.Create(32, 32), ImageIdex));
end;

procedure TInteractiveMap.SetMouseDown(const Value: Boolean);
begin
  FMousePosition.Down := Value;
  if Value then
    Background.Cursor := crSizeAll
  else
    Background.Cursor := crDefault;
end;

procedure TInteractiveMap.HideMarkerInfo;
begin
  MarkerPanel.Visible := False;
end;

function TInteractiveMap.NormalizePosition(const Left, Top: Integer):  TPoint;
const
  ReserveMapID = 5;
  Angle = 0.268; // 0.523599; //0.523599;  0.268
var
  x, y: Single;
  Offset: Single;
begin
  x := Left;
  y := Top;
  if Map.ID = ReserveMapID then begin
    x := Trunc(x * cos(Angle) - y * sin(Angle));
    y := Trunc(x * sin(Angle) + y * cos(Angle));
  end;

  Offset := Abs((Map.Top - y) / (Map.Bottom - Map.Top));
  Result.Top := Trunc(Bitmap.Height * Offset); // - MarkerHeight div 2;
  Offset := Abs((Map.Left - x) / (Map.Right - Map.Left));
  Result.Left := Trunc(Bitmap.Width * Offset); // - MarkerWidth div 2;
end;

procedure TInteractiveMap.Refresh;
begin
  FMapWrapper.Refresh;
end;

{$IFDEF DEBUG}
procedure TInteractiveMap.TestPosition;
var
  p: TPoint;
begin
  Inc(FTestIndex);
  if FTestIndex >= Length(FTestData) then
    FTestIndex := 0;

  p := FMapWrapper.ExtractPoint(FTestData[FTestIndex]);
  FMapWrapper.DrawPoint(p);
end;
{$ENDIF}

procedure TInteractiveMap.BackgroundDblClick(Sender: TObject);
begin
  SetMouseDown(False);

  if Assigned(FOnDoubleClick) then
    FOnDoubleClick(Self);
end;

procedure TInteractiveMap.BackgroundMouseDown(Sender: TObject; Button: TMouseButton; Shift: TShiftState; X, Y: Single);
begin
  SetMouseDown(True);
  FMousePosition.X := X;
  FMousePosition.Y := Y;
  MarkerPanel.Visible := False;

  if Assigned(FOnMouseDown) then
    FOnMouseDown(Self);
end;

procedure TInteractiveMap.BackgroundMouseLeave(Sender: TObject);
begin
  SetMouseDown(False);
end;

procedure TInteractiveMap.BackgroundMouseMove(Sender: TObject; Shift: TShiftState; X, Y: Single);
const
  Offset = 5;
var
  DeltaX, DeltaY: Single;
begin
  if not FMousePosition.Down then
    Exit;

//{$IFNDEF DEBUG}
//  Label1.Caption := Sender.ClassName + '.Position: (' + IntToStr(X) + ', ' + IntToStr(Y) + ')';
//  Label2.Caption := 'Image: (' + IntToStr(imLocalMap.Left) + ', ' + IntToStr(imLocalMap.Top) + ')';
//{$ENDIF}

  DeltaX := X - FMousePosition.X;
  DeltaY := Y - FMousePosition.Y;

  if (Abs(DeltaX) > Offset) or (Abs(DeltaY) > Offset) then begin
    Background.Position.X := Background.Position.X + DeltaX;
    Background.Position.Y := Background.Position.Y + DeltaY;

    FMousePosition.X := X - DeltaX;
    FMousePosition.Y := Y - DeltaY;
  end;
end;

procedure TInteractiveMap.BackgroundMouseUp(Sender: TObject; Button: TMouseButton; Shift: TShiftState; X, Y: Single);
begin
  SetMouseDown(False);
end;

procedure TInteractiveMap.Center;
begin
  Background.Position.X := (MainContainer.Width - Background.Width) / 2;
  Background.Position.Y := (MainContainer.Height - Background.Height) / 2;
end;

procedure TInteractiveMap.ZoomIn;
begin
  FMapWrapper.ZoomIn;
end;

procedure TInteractiveMap.ZoomOut;
begin
  FMapWrapper.ZoomOut;
end;

end.
