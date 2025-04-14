﻿unit ME.MainForm;

interface

uses
  System.SysUtils, System.Types, System.UITypes, System.Classes, System.Variants,
  System.IOUtils, Generics.Collections, FMX.Types, FMX.Controls, FMX.Forms,
  FMX.Graphics, FMX.Dialogs, FMX.Controls.Presentation, FMX.StdCtrls,
  System.ImageList, FMX.ImgList, FMX.Objects, FMX.Layouts,
  Map.Data.Types, ME.Filter.Map, ME.Frame.MapData, FMX.TabControl,
  ME.Grid.Resources, ME.DB.Map, ME.Grid.QuestResources, ME.Frame.Quest,
  ME.Frame.QuestPart;

type
  TMainForm = class(TForm)
    MainContainer: TTabControl;
    GeneralTab: TTabItem;
    QuestItemsTab: TTabItem;
    TopLayout: TLayout;
    Panel1: TPanel;
    buExpot: TButton;
    buExportToDB: TButton;
    Button1: TButton;
    buReconnect: TButton;
    QuestsTab: TTabItem;
    QuestLayout: TLayout;
    QuestPartsLayout: TLayout;
    Splitter1: TSplitter;
    procedure FormCreate(Sender: TObject);
    procedure FormShow(Sender: TObject);
    procedure buExpotClick(Sender: TObject);
    procedure MapChanged(const MapID: Variant);
    procedure buExportToDBClick(Sender: TObject);
    procedure Button1Click(Sender: TObject);
    procedure buReconnectClick(Sender: TObject);
  private
//    FMapPanel: TfrMap;
    FMapFilter: TMapFilter;
    FMapData: TfrMapData;
    FResourcesGrid: TQuestResourcesGrid;
    FQuestGrid: TfrQuest;
    FQuestPartGrid: TfrQuestPartGrid;

    procedure ApplicationInit;
    procedure OnQuestChanged(const QuestID: Variant);
  public
  end;

var
  MainForm: TMainForm;

implementation

uses
  ME.DB.Utils, App.Service, ME.Service.Layer, ME.Service.Map,
  ME.DB.Marker, ME.Service.Marker, ME.Service.Quest, ME.Service.Resource,
  Map.Data.Service, Map.Data.Classes;

{$R *.fmx}

// id, icon, name, выходы чвк(кол-во), выходы дикого(кол-во), выходы совместные(кол-во), квесты (кол-во)
// QuestResources

procedure TMainForm.FormCreate(Sender: TObject);
begin
  AppService.LoadParams;
//  AppService.LoadDataFromJSON;

  Self.Caption := '[Map Editor] JSON storage';
//  Self.Caption := '[Maps Editor] Database = "' + AppService.Database + '"';

//  FMapPanel := TfrMap.Create(Self);
//  FMapPanel.Parent := Self;
//  FMapPanel.Align := TAlignLayout.Client;
////  FMapPanel.OnChange := OnMapChanged;

  FMapFilter := TMapFilter.Create(Self);
  FMapFilter.Parent := TopLayout;
  FMapFilter.Position.X := 20;
  FMapFilter.Position.Y := 0;
  FMapFilter.OnMapChanged := MapChanged;

  FMapData := TfrMapData.Create(Self);
  FMapData.Parent := GeneralTab;
  FMapData.Align := TAlignLayout.Client;

  FResourcesGrid := TQuestResourcesGrid.Create(Self);
  FResourcesGrid.Parent := QuestItemsTab;
  FResourcesGrid.Align := TAlignLayout.Client;
  FResourcesGrid.Sorted := True;

  FQuestGrid := TfrQuest.Create(Self);
  FQuestGrid.Parent := QuestLayout;
  FQuestGrid.Align := TAlignLayout.Client;
  FQuestGrid.OnQuestChanged := OnQuestChanged;

  FQuestPartGrid := TfrQuestPartGrid.Create(Self);
  FQuestPartGrid.Parent := QuestPartsLayout;
  FQuestPartGrid.Align := TAlignLayout.Client;

  MainContainer.ActiveTab := GeneralTab;
end;

procedure TMainForm.FormShow(Sender: TObject);
begin
  ApplicationInit;
end;

procedure TMainForm.ApplicationInit;
begin
  AppService.ConnectToDB;

  FMapFilter.Init;
  FResourcesGrid.Init(nil);
  FQuestGrid.Init(Null);
//  FMapPanel.Init;
end;

procedure TMainForm.OnQuestChanged(const QuestID: Variant);
begin
  FQuestPartGrid.Init(Null, QuestID);
end;

procedure TMainForm.MapChanged(const MapID: Variant);
begin
  FMapData.Init(MapID);
end;

procedure TMainForm.buExpotClick(Sender: TObject);
var
  FileName: string;
begin
  FileName := System.IOUtils.TPath.Combine(AppService.Options.DataPath, 'data.json');
//  TJSONDataExport.SaveToFile(FileName, DataService.Items);

  ShowMessage('Done');
end;

procedure TMainForm.buExportToDBClick(Sender: TObject);
begin
//  TDBDataImport.Load(DataService.Items);

  ShowMessage('Done');
end;

procedure TMainForm.Button1Click(Sender: TObject);
begin
  ResourceService.ExportFromDB;

  ShowMessage('Done');
end;

procedure TMainForm.buReconnectClick(Sender: TObject);
var
  MapID: Variant;
begin
  MapID := FMapFilter.MapID;
  ApplicationInit;
  FMapFilter.MapID := MapID;
end;

end.
