unit ME.GUI.PictureList;

interface

uses
  System.SysUtils, System.Types, System.UITypes, System.Classes, System.Variants,
  FMX.Types, FMX.Graphics, FMX.Controls, FMX.Forms, FMX.Dialogs, FMX.StdCtrls,
  FMX.Layouts, Generics.Collections, ME.GUI.PictureItem, FMX.Objects;

type
  TListDirection = (Vertical, Horizontal);
  TChangedItemEvent = procedure (ItemIndex: Integer) of object;

  TPictureList = class(TFrame)
    Background: TRectangle;
    MainContainer: TFramedScrollBox;
    StyleBook: TStyleBook;

    procedure MainContainerResize(Sender: TObject);
    procedure MainContainerMouseWheel(Sender: TObject; Shift: TShiftState; WheelDelta: Integer; var Handled: Boolean);
  private
    FItems: TObjectList<TPictureItemItem>;
    FListDirection: TListDirection;
    FItemHeight: Single;
    FItemWidth: Single;
    FSelectedIndex: Integer;
    FItemColor: TAlphaColor;
    FFocusedColor: TAlphaColor;
    FSelectedColor: TAlphaColor;
    FHideFocus: Boolean;
    FHideSelect: Boolean;
    FStretch: Boolean;
    FDelimerWidth: Integer;
    FStrokeThickness: Integer;
    FOnChangeItem: TChangedItemEvent;

    procedure PreviewIconItemClick(Sender: TObject);
    function GetCount: Integer;
    function GetPreviewIconItem(Index: Integer): TPictureItemItem;
    procedure SetSelectedIndex(const Value: Integer);
    function GetIItemWidth: Single;
    function GetItemHeight: Single;
    procedure SetItemHeight(const Value: Single);
    procedure SetItemWidth(const Value: Single);
    function GetBackgroundColor: TAlphaColor;
    procedure SetBackgroundColor(const Value: TAlphaColor);
    procedure SetListDirection(const Value: TListDirection);
    procedure SetItemMargins(const Item: TPictureItemItem);
    procedure SetItemSize(const Item: TPictureItemItem);
    procedure SetItemPosition(const Item: TPictureItemItem);
    function GetStrokeThickness: Integer;
  protected
    procedure SetHeight(const Value: Single); override;
    procedure SetWidth(const Value: Single); override;
  public
    constructor Create(AOwner: TComponent); override;
    destructor Destroy; override;

    procedure Add(const Title: string; const Bitmap: TBitmap); overload;
    procedure Add(const Bitmap: TBitmap); overload;
    procedure Clear;

    procedure OnMouseWheel(WheelDelta: Integer);

    property Count: Integer read GetCount;
    property Items[Index: Integer]: TPictureItemItem read GetPreviewIconItem;
    property ListDirection: TListDirection read FListDirection write SetListDirection;
    property SelectedIndex: Integer read FSelectedIndex write SetSelectedIndex;
    property ItemHeight: Single read GetItemHeight write SetItemHeight;
    property ItemWidth: Single read GetIItemWidth write SetItemWidth;
    property BackgroundColor: TAlphaColor read GetBackgroundColor write SetBackgroundColor;
    property ItemColor: TAlphaColor read FItemColor write FItemColor;
    property FocusedColor: TAlphaColor read FFocusedColor write FFocusedColor;
    property SelectedColor: TAlphaColor read FSelectedColor write FSelectedColor;
    property HideFocus: Boolean read FHideFocus write FHideFocus;
    property HideSelect: Boolean read FHideSelect write FHideSelect;
    property Stretch: Boolean read FStretch write FStretch;
    property DelimerWidth: Integer read FDelimerWidth write FDelimerWidth;
    property StrokeThickness: Integer read GetStrokeThickness write FStrokeThickness;
    property OnChangeItem: TChangedItemEvent read FOnChangeItem write FOnChangeItem;
  end;

implementation

{$R *.fmx}

{ TPreviewIconList }

constructor TPictureList.Create(AOwner: TComponent);
begin
  inherited;

  FItems := TObjectList<TPictureItemItem>.Create;
  FListDirection := TListDirection.Horizontal;
  ItemHeight := 64;
  ItemWidth := 64;
  BackgroundColor := $FF252525;
  ItemColor := $FF252525;
  FocusedColor := $FF514ee6; //$FF383838;
  SelectedColor := $FF0020E0;
  FHideFocus := False;
  FHideSelect := False;
  FStretch := False;
  FStrokeThickness := 2;
  FDelimerWidth := 5;

  FOnChangeItem := nil;
end;

destructor TPictureList.Destroy;
begin
  FOnChangeItem := nil;
  FItems.Free;

  inherited;
end;

function TPictureList.GetCount: Integer;
begin
  Result := FItems.Count;
end;

function TPictureList.GetItemHeight: Single;
begin
  Result := FItemHeight; //MainContainer.ItemHeight;
end;

procedure TPictureList.SetItemHeight(const Value: Single);
begin
  //MainContainer.ItemHeight := Value;
  FItemHeight := Value;

  case ListDirection of
    TListDirection.Vertical: begin
      Self.Width := Value + StrokeThickness;
      MainContainer.Width := Value + StrokeThickness;
    end;
    TListDirection.Horizontal: begin
      Self.Height := Value + StrokeThickness;
      MainContainer.Height := Value + StrokeThickness;
    end;
  end;
end;

function TPictureList.GetIItemWidth: Single;
begin
  Result := FItemWidth; // MainContainer.ItemWidth;
end;

procedure TPictureList.SetItemWidth(const Value: Single);
begin
//  MainContainer.ItemWidth := Value;
  FItemWidth := Value;
end;

procedure TPictureList.SetListDirection(const Value: TListDirection);
begin
  if FListDirection = Value then
    Exit;

  FListDirection := Value;
  case FListDirection of
    TListDirection.Vertical:
      MainContainer.Align := TAlignLayout.Left;
    TListDirection.Horizontal:
      MainContainer.Align := TAlignLayout.Top;
  end;
end;

function TPictureList.GetBackgroundColor: TAlphaColor;
begin
  Result := Background.Fill.Color;
end;

procedure TPictureList.SetBackgroundColor(const Value: TAlphaColor);
begin
  Background.Fill.Color := Value;
end;

procedure TPictureList.SetItemMargins(const Item: TPictureItemItem);
begin
  case FListDirection of
    TListDirection.Vertical: begin
      if Item.Index = 0 then
        Item.Margins.Top := DelimerWidth
      else
        Item.Margins.Top := 0;

      Item.Margins.Left := StrokeThickness;
      Item.Margins.Bottom := 0;
      Item.Margins.Right := StrokeThickness;
    end;
    TListDirection.Horizontal: begin
      if Item.Index = 0 then
        Item.Margins.Left := DelimerWidth
      else
        Item.Margins.Left := 0;

      Item.Margins.Top := StrokeThickness;
      Item.Margins.Bottom := StrokeThickness;
      Item.Margins.Right := 0;
    end;
  end;
end;

procedure TPictureList.SetItemSize(const Item: TPictureItemItem);
var
  Height: Single;
  Width: Single;
begin
  if Stretch then begin
    case FListDirection of
      TListDirection.Vertical: begin
        Item.Width := MainContainer.Width - StrokeThickness * 2;

        Height := Item.Width * (Item.Picture.Height / Item.Picture.Width) + StrokeThickness * 2;
        if Item.Picture.Height < Height then
          Item.Height := Item.Picture.Height + StrokeThickness * 2
        else
          Item.Height := Height;
      end;
      TListDirection.Horizontal: begin
        Item.Height := MainContainer.Height - StrokeThickness * 2;

        Width := Item.Height * (Item.Picture.Width / Item.Picture.Height) + StrokeThickness * 2;
        if Item.Picture.Width < Width then
          Item.Width := Item.Picture.Width + StrokeThickness * 2
        else
          Item.Width := Width;
      end;
    end;
  end
  else begin
    Item.Width := ItemWidth;
    Item.Height := ItemHeight;
  end;
end;

procedure TPictureList.SetItemPosition(const Item: TPictureItemItem);
begin
  case FListDirection of
    TListDirection.Vertical: begin
      Item.Position.X := Item.Margins.Left;
      Item.Position.Y := (Item.Width + Item.Margins.Top + Item.Margins.Bottom + DelimerWidth) * Count;
    end;
    TListDirection.Horizontal: begin
      Item.Position.X := (Item.Width + Item.Margins.Left + Item.Margins.Right + DelimerWidth) * Count;
      Item.Position.Y := Item.Margins.Top;
    end;
  end;
end;

function TPictureList.GetPreviewIconItem(Index: Integer): TPictureItemItem;
begin
  Result := FItems[Index];
end;

procedure TPictureList.SetSelectedIndex(const Value: Integer);
var
  Item: TPictureItemItem;
begin
  if Count = 0 then begin
    FSelectedIndex := -1;
    Exit;
  end;

  if (Value < 0) or (Value >= Count) then
    Exit;

  FSelectedIndex := Value;
  for Item in FItems do
    Item.IsSelected := (Item.Index = Value);

  if Assigned(FOnChangeItem) then
    FOnChangeItem(FSelectedIndex);
end;

function TPictureList.GetStrokeThickness: Integer;
begin
  if HideFocus and HideSelect then
    Result := 0
  else
    Result := FStrokeThickness;
end;

procedure TPictureList.SetHeight(const Value: Single);
begin
  inherited;

  case FListDirection of
    TListDirection.Horizontal:
      MainContainer.Height := Value;
  end;
end;

procedure TPictureList.SetWidth(const Value: Single);
begin
  inherited;

  case FListDirection of
    TListDirection.Vertical:
      MainContainer.Width := Value;
  end;
end;

procedure TPictureList.Add(const Title: string; const Bitmap: TBitmap);
var
  Item: TPictureItemItem;
begin
  Item := TPictureItemItem.Create(Self);
  try
    Item.BeginUpdate;
    try
      Item.Parent := MainContainer;
      Item.Name := 'PreviewIconItem' + IntToStr(Count + 1);
      Item.Title := Title;
      Item.Picture := Bitmap;
      Item.HideFocus := HideFocus;
      Item.HideSelect := HideSelect;
      Item.Stretch := Stretch;
      Item.BackgroundColor := ItemColor;
      Item.FocusedColor := FocusedColor;
      Item.SelectedColor := SelectedColor;
      Item.StrokeThickness := StrokeThickness;

      SetItemMargins(Item);
      SetItemSize(Item);
      SetItemPosition(Item);
      case FListDirection of
        TListDirection.Vertical:
          Item.Align := TAlignLayout.Top;
        TListDirection.Horizontal:
          Item.Align := TAlignLayout.Left;
      end;

      if HideSelect then
        Item.Cursor := crDefault
      else
        Item.Cursor := crHandPoint;

      Item.OnClick := PreviewIconItemClick;
    finally
      Item.EndUpdate;
    end;
  finally
    Item.Index := FItems.Add(Item);
  end;
end;

procedure TPictureList.Add(const Bitmap: TBitmap);
begin
  Add('', Bitmap);
end;

procedure TPictureList.Clear;
begin
  FItems.Clear;
end;

procedure TPictureList.OnMouseWheel(WheelDelta: Integer);
var
  Offset: Single;
  Item: TPictureItemItem;
begin
  if Count <= 1 then
    Exit;

  if WheelDelta > 0 then
    SelectedIndex := SelectedIndex - 1
  else
    SelectedIndex := SelectedIndex + 1;

  Item := Items[SelectedIndex];
  Offset := MainContainer.Width - Item.Position.X - Item.Width;
  MainContainer.ScrollBy(Offset, 0);
end;

procedure TPictureList.MainContainerResize(Sender: TObject);
begin
  if MainContainer.Height > Height then
    MainContainer.Height := Height;

  if MainContainer.Width > Width then
    MainContainer.Width := Width;
end;

procedure TPictureList.PreviewIconItemClick(Sender: TObject);
begin
  SelectedIndex := TPictureItemItem(Sender).Index;
end;

procedure TPictureList.MainContainerMouseWheel(Sender: TObject; Shift: TShiftState; WheelDelta: Integer; var Handled: Boolean);
begin
  Handled := True;
  OnMouseWheel(WheelDelta);
end;

end.
