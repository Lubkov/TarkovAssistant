﻿unit TM.MainForm;

interface

uses
  System.SysUtils, System.Types, System.UITypes, System.Classes, System.Variants,
  System.IOUtils, Generics.Collections, FMX.Types, FMX.Controls, FMX.Forms, FMX.ListBox,
  FMX.Graphics, FMX.Dialogs, TM.Form.Wrapper, FMX.Controls.Presentation, FMX.StdCtrls,
  FMX.Layouts, System.ImageList, FMX.ImgList, FMX.Objects, System.Actions, FMX.ActnList,
  TM.Map.Wrapper, TM.Frame.Location, TM.Frame.MarkerFilter,
  Map.Frame.InteractiveMap, TM.Frame.Options,
  ME.DB.Map, ME.Options;

type
  TMainForm = class(TForm)
    MainStyleBook: TStyleBook;
    ImageList32: TImageList;
    MapControlLayout: TLayout;
    buFullScreen: TSpeedButton;
    buZoomIn: TSpeedButton;
    buZoomOut: TSpeedButton;
    buCentreMap: TSpeedButton;
    buMapFilters: TSpeedButton;
    MainActionList: TActionList;
    acFullScreen: TAction;
    MapTagImages: TImageList;
    acZoomIn: TAction;
    acZoomOut: TAction;
    acChoiceLocation: TAction;
    acCentreMap: TAction;
    acMarkerFilterOpen: TAction;
    buChoiceLocation: TSpeedButton;
    LocationPanel: TPanel;
    MarkerFilterPanel: TPanel;
    buPositionTest: TSpeedButton;
    acTestPosition: TAction;
    buOptions: TSpeedButton;
    acShowOptions: TAction;
    OptionsPanel: TPanel;

    procedure FormCreate(Sender: TObject);
    procedure FormDestroy(Sender: TObject);
    procedure acFullScreenExecute(Sender: TObject);
    procedure FormKeyDown(Sender: TObject; var Key: Word; var KeyChar: Char; Shift: TShiftState);
    procedure acZoomInExecute(Sender: TObject);
    procedure acZoomOutExecute(Sender: TObject);
    procedure acChoiceLocationExecute(Sender: TObject);
    procedure acCentreMapExecute(Sender: TObject);
    procedure acMarkerFilterOpenExecute(Sender: TObject);
    procedure FormResize(Sender: TObject);
    procedure buToolButtonMouseMove(Sender: TObject; Shift: TShiftState; X, Y: Single);
    procedure acTestPositionExecute(Sender: TObject);
    procedure acShowOptionsExecute(Sender: TObject);
  private
    FFormWrapper: TFormWrapper;
    FLocationGrid: TLocationGrid;
    FMarkerFilterList: TMarkerFilterList;
    FInteractiveMap: TInteractiveMap;
    FOptionsFrame: TOptionsFrame;

    procedure HideAllPanels(Sender: TObject);
    procedure HideOptionsPanel(Sender: TObject);
    procedure SetFullScreenMode(const Value: Boolean);
    procedure OnLocationChanged(const Value: TDBMap);
    procedure MarkerFilterListOnClose(Sender: TObject);
    procedure OnInteractiveMapDblClick(Sender: TObject);
  public
  end;

var
  MainForm: TMainForm;

implementation

uses
  App.Service, Map.Data.Service, Map.CursorService;

{$R *.fmx}

procedure TMainForm.FormCreate(Sender: TObject);
const
  BackgroundColor = $FF0F0F0F;
begin
  AppService.LoadParams;
  AppService.ConnectToDB;
  AppService.LoadDataFromDB;

  Self.Caption := '["Escape of Tarkov" position tracking]';
  Self.Fill.Color := BackgroundColor;
  Self.Fill.Kind := TBrushKind.Solid;

  LocationPanel.Visible := False;
  MarkerFilterPanel.Visible := False;

  FFormWrapper := TFormWrapper.Create(Self);

  FInteractiveMap := TInteractiveMap.Create(Self);
  FInteractiveMap.Parent := Self;
  FInteractiveMap.Align := TAlignLayout.Contents;
  FInteractiveMap.SendToBack;
  FInteractiveMap.OnDoubleClick := OnInteractiveMapDblClick;
  FInteractiveMap.OnMouseDown := HideAllPanels;

  FMarkerFilterList := TMarkerFilterList.Create(Self);
  FMarkerFilterList.Parent := MarkerFilterPanel;
  FMarkerFilterList.Align := TAlignLayout.Client;
  FMarkerFilterList.Init(FInteractiveMap.MarkerFilter);
  FMarkerFilterList.OnClose := MarkerFilterListOnClose;

  FLocationGrid := TLocationGrid.Create(Self);
  FLocationGrid.Parent := LocationPanel;
  FLocationGrid.Align := TAlignLayout.Client;
  FLocationGrid.Init;
  FLocationGrid.OnLocationChanged := OnLocationChanged;

  FOptionsFrame := TOptionsFrame.Create(Self);
  FOptionsFrame.Parent := OptionsPanel;
  FOptionsFrame.Align := TAlignLayout.Client;
  FOptionsFrame.Init(AppService.Options);
  FOptionsFrame.OnClose := HideOptionsPanel;

  buPositionTest.Visible := {$IFDEF DEBUG}True{$ELSE}False{$ENDIF};
end;

procedure TMainForm.FormDestroy(Sender: TObject);
begin
  FFormWrapper.Free;
  FreeAndNil(FLocationGrid);
end;

procedure TMainForm.FormKeyDown(Sender: TObject; var Key: Word; var KeyChar: Char; Shift: TShiftState);
begin
  case Key of
    vkEscape:
      if LocationPanel.Visible then
        LocationPanel.Visible := False
      else
      if MarkerFilterPanel.Visible then
        MarkerFilterPanel.Visible := False
      else
      if FInteractiveMap.MarkerInfoVisible then
        FInteractiveMap.HideMarkerInfo
      else
      if OptionsPanel.Visible then
        HideOptionsPanel(Sender)
      else
        SetFullScreenMode(False);
    vkF11:
      SetFullScreenMode(True);
  end;
end;

procedure TMainForm.FormResize(Sender: TObject);
begin
  HideAllPanels(Sender);
end;

procedure TMainForm.buToolButtonMouseMove(Sender: TObject; Shift: TShiftState; X, Y: Single);
begin
  FInteractiveMap.SetMouseDown(False);
end;

procedure TMainForm.acFullScreenExecute(Sender: TObject);
begin
  SetFullScreenMode(not FFormWrapper.FullScreen);
end;

procedure TMainForm.HideAllPanels;
begin
  LocationPanel.Visible := False;
  MarkerFilterPanel.Visible := False;
  FInteractiveMap.HideMarkerInfo;
  HideOptionsPanel(Sender);

  Application.ProcessMessages;
end;

procedure TMainForm.HideOptionsPanel(Sender: TObject);
begin
  if OptionsPanel.Visible then begin
    FOptionsFrame.Save;
    FInteractiveMap.Refresh;
  end;

  OptionsPanel.Visible := False;
end;

procedure TMainForm.SetFullScreenMode(const Value: Boolean);
begin
  FFormWrapper.FullScreen := Value;
//  MapControlLayout.Visible := not FFormWrapper.FullScreen;
//  FormResize(Self);
end;

procedure TMainForm.OnLocationChanged(const Value: TDBMap);
begin
  HideAllPanels(Self);

  TMapCursorService.Cursor := crHourGlass;
  try
    FInteractiveMap.Map := Value;
  finally
    TMapCursorService.Cursor := crDefault;
  end;
end;

procedure TMainForm.MarkerFilterListOnClose(Sender: TObject);
begin
  MarkerFilterPanel.Visible := False;
end;

procedure TMainForm.acZoomInExecute(Sender: TObject);
begin
  FInteractiveMap.ZoomIn;
end;

procedure TMainForm.acZoomOutExecute(Sender: TObject);
begin
  FInteractiveMap.ZoomOut;
end;

procedure TMainForm.acChoiceLocationExecute(Sender: TObject);
var
  Height: Single;
  Width: Single;
  OffsetX: Single;
  Visible: Boolean;
begin
  Visible := LocationPanel.Visible;
  HideAllPanels(Sender);

  OffsetX := MapControlLayout.Position.X + MapControlLayout.Width;
  Height := Self.ClientHeight;
  Width := Self.ClientWidth - OffsetX - 40;

  if Width < FLocationGrid.MaxWidth then
    LocationPanel.Width := Width
  else
    LocationPanel.Width := FLocationGrid.MaxWidth;

  if Height < FLocationGrid.MaxHeight then
    LocationPanel.Height := Height
  else
    LocationPanel.Height := FLocationGrid.MaxHeight;

  LocationPanel.Position.X := (Width / 2) - (LocationPanel.Width / 2) + OffsetX + 20;
  LocationPanel.Position.Y := (Height / 2) - (LocationPanel.Height / 2);
  LocationPanel.Visible := not Visible;
end;

procedure TMainForm.OnInteractiveMapDblClick(Sender: TObject);
begin
  SetFullScreenMode(not FFormWrapper.FullScreen);
end;

procedure TMainForm.acCentreMapExecute(Sender: TObject);
begin
  FInteractiveMap.Center;
end;

procedure TMainForm.acMarkerFilterOpenExecute(Sender: TObject);
var
  Height: Single;
begin
  HideAllPanels(Sender);

  MarkerFilterPanel.Position.X := MapControlLayout.Position.X + MapControlLayout.Width + 10;
  MarkerFilterPanel.Position.Y := MapControlLayout.Position.Y;
  MarkerFilterPanel.Height := 550;
  MarkerFilterPanel.Width := 340;

  Height := Self.ClientHeight - MarkerFilterPanel.Position.Y - 10;
  if Height < FMarkerFilterList.MaxHeight then
    MarkerFilterPanel.Height := Height
  else
    MarkerFilterPanel.Height := FMarkerFilterList.MaxHeight;

  MarkerFilterPanel.Visible := not MarkerFilterPanel.Visible;
end;

procedure TMainForm.acTestPositionExecute(Sender: TObject);
begin
  FInteractiveMap.TestPosition;
end;

procedure TMainForm.acShowOptionsExecute(Sender: TObject);
var
  Visible: Boolean;
begin
  Visible := OptionsPanel.Visible;
  HideAllPanels(Sender);

  OptionsPanel.Height := FOptionsFrame.MaxHeight;
  OptionsPanel.Width := FOptionsFrame.MaxWidth;
  OptionsPanel.Position.X := Self.Width / 2 - OptionsPanel.Width / 2;
  OptionsPanel.Position.Y := Self.Height / 2 - OptionsPanel.Height / 2 - 28;
  OptionsPanel.Visible := not Visible;
end;

end.
