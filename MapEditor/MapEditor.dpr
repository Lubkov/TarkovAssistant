program MapEditor;

uses
  System.StartUpCopy,
  FMX.Forms,
  ME.MainForm in 'Source\ME.MainForm.pas' {MainForm},
  ME.DB.DAO in '..\Source\Common\ME.DB.DAO.pas',
  ME.DB.Entity in '..\Source\Common\ME.DB.Entity.pas',
  ME.DB.Service in '..\Source\Common\ME.DB.Service.pas',
  ME.DB.Utils in '..\Source\Common\ME.DB.Utils.pas',
  ME.Del.Form.Presenter in '..\Source\Common\ME.Del.Form.Presenter.pas',
  ME.Dialog.Presenter in '..\Source\Common\ME.Dialog.Presenter.pas',
  ME.Edit.Form.Presenter in '..\Source\Common\ME.Edit.Form.Presenter.pas',
  ME.DAO.Map in '..\Source\DAO\ME.DAO.Map.pas',
  ME.DAO.Layer in '..\Source\DAO\ME.DAO.Layer.pas',
  ME.DB.Map in '..\Source\Entity\ME.DB.Map.pas',
  ME.DB.Layer in '..\Source\Entity\ME.DB.Layer.pas',
  ME.Service.Map in '..\Source\Service\ME.Service.Map.pas',
  ME.Service.Layer in '..\Source\Service\ME.Service.Layer.pas',
  ME.Presenter.Map in 'Source\GUI\ME.Presenter.Map.pas',
  ME.Frame.Layer in 'Source\GUI\Frame\ME.Frame.Layer.pas' {frLayerList: TFrame},
  ME.Presenter.Layer in 'Source\GUI\ME.Presenter.Layer.pas',
  ME.Edit.Form in '..\Source\Common\ME.Edit.Form.pas' {EditForm},
  ME.Edit.Map in 'Source\GUI\Dialog\ME.Edit.Map.pas' {edMap},
  ME.Edit.Layer in 'Source\GUI\Dialog\ME.Edit.Layer.pas' {edLayer},
  ME.Frame.Picture in 'Source\GUI\Frame\ME.Frame.Picture.pas' {frPicture: TFrame},
  ME.DB.Marker in '..\Source\Entity\ME.DB.Marker.pas',
  ME.DAO.Marker in '..\Source\DAO\ME.DAO.Marker.pas',
  ME.Service.Marker in '..\Source\Service\ME.Service.Marker.pas',
  ME.Frame.Marker in 'Source\GUI\Frame\ME.Frame.Marker.pas' {frMarkerGrid: TFrame},
  ME.Edit.Marker in 'Source\GUI\Dialog\ME.Edit.Marker.pas' {edMarker},
  ME.Presenter.Marker in 'Source\GUI\ME.Presenter.Marker.pas',
  ME.DB.Quest in '..\Source\Entity\ME.DB.Quest.pas',
  ME.DAO.Quest in '..\Source\DAO\ME.DAO.Quest.pas',
  ME.Service.Quest in '..\Source\Service\ME.Service.Quest.pas',
  ME.Frame.Quest in 'Source\GUI\Frame\ME.Frame.Quest.pas' {frQuest: TFrame},
  ME.Edit.Quest in 'Source\GUI\Dialog\ME.Edit.Quest.pas' {edQuest},
  ME.Presenter.Quest in 'Source\GUI\ME.Presenter.Quest.pas',
  ME.Edit.QuestPart in 'Source\GUI\Dialog\ME.Edit.QuestPart.pas' {edQuestPart},
  ME.Frame.QuestPart in 'Source\GUI\Frame\ME.Frame.QuestPart.pas' {frQuestPartGrid: TFrame},
  ME.Presenter.QuestPart in 'Source\GUI\ME.Presenter.QuestPart.pas',
  LocalMap in '..\Source\Entity\LocalMap.pas',
  App.DB.Connection in '..\Source\Common\App.DB.Connection.pas',
  App.SQLite.Connection in '..\Source\Common\App.SQLite.Connection.pas',
  App.Service in '..\Source\Common\App.Service.pas',
  Map.Data.Classes in '..\Source\Data\Map.Data.Classes.pas',
  Map.Data.Service in '..\Source\Data\Map.Data.Service.pas',
  Map.Data.Types in '..\Source\Data\Map.Data.Types.pas',
  ME.Filter.Map in 'Source\GUI\Frame\ME.Filter.Map.pas' {MapFilter: TFrame},
  ME.Frame.MapData in 'Source\GUI\Frame\ME.Frame.MapData.pas' {frMapData: TFrame},
  ME.Presenter.QuestItem in 'Source\GUI\ME.Presenter.QuestItem.pas',
  ME.DB.Resource in '..\Source\Entity\ME.DB.Resource.pas',
  ME.DAO.Resource in '..\Source\DAO\ME.DAO.Resource.pas',
  ME.Service.Resource in '..\Source\Service\ME.Service.Resource.pas',
  ME.Grid.Resources in 'Source\GUI\Frame\ME.Grid.Resources.pas' {ResourcesDBGrid: TFrame},
  ME.DB.Edit.Resource in 'Source\GUI\Dialog\ME.DB.Edit.Resource.pas' {edDBResource},
  ME.DB.Presenter.Resource in 'Source\GUI\ME.DB.Presenter.Resource.pas',
  ME.DB.QuestItem in '..\Source\Entity\ME.DB.QuestItem.pas',
  ME.DAO.QuestItem in '..\Source\DAO\ME.DAO.QuestItem.pas',
  ME.Service.QuestItem in '..\Source\Service\ME.Service.QuestItem.pas',
  ME.Grid.Screenshots in 'Source\GUI\Frame\ME.Grid.Screenshots.pas' {ScreenshotsGrid: TFrame},
  ME.MemGrid.Screenshots in 'Source\GUI\Frame\ME.MemGrid.Screenshots.pas' {ScreenshotsMemGrid},
  ME.Grid.QuestItems in 'Source\GUI\Frame\ME.Grid.QuestItems.pas' {QuestItemsGrid: TFrame},
  ME.MemGrid.QuestItems in 'Source\GUI\Frame\ME.MemGrid.QuestItems.pas' {QuestItemsMemGrid: TFrame},
  ME.Edit.QuestItem in 'Source\GUI\Dialog\ME.Edit.QuestItem.pas' {edQuestItem},
  ME.Grid.QuestResources in 'Source\GUI\Frame\ME.Grid.QuestResources.pas' {QuestResourcesGrid: TFrame},
  ME.Presenter.Screenshot in 'Source\GUI\ME.Presenter.Screenshot.pas',
  ME.Service.Profile in '..\Source\Service\ME.Service.Profile.pas',
  ME.Service.QuestTracker in '..\Source\Service\ME.Service.QuestTracker.pas',
  ME.Filter.Profile in '..\Source\GUI\Frame\ME.Filter.Profile.pas' {ProfileFilter: TFrame},
  ME.Presenter.Profile in '..\Source\GUI\Presenter\ME.Presenter.Profile.pas',
  ME.Edit.Profile in '..\Source\GUI\Edit\ME.Edit.Profile.pas' {edProfile},
  ME.Dialog.Message in '..\Source\Common\ME.Dialog.Message.pas' {edMessage},
  App.Entity in '..\Source\Common\App.Entity.pas',
  ME.Profile in '..\Source\Entity\ME.Profile.pas',
  ME.QuestTracker in '..\Source\Entity\ME.QuestTracker.pas',
  ME.Options in '..\Source\Entity\ME.Options.pas',
  ME.Trader in '..\Source\Entity\ME.Trader.pas',
  ME.Form.Filter in '..\Source\Common\ME.Form.Filter.pas' {FormFilter: TFrame},
  ME.Filter.Trader in 'Source\GUI\Filter\ME.Filter.Trader.pas' {TraderFilter: TFrame};

{$R *.res}

begin
{$IFDEF DEBUG}
  ReportMemoryLeaksOnShutdown := True;
{$ENDIF}

  Application.Initialize;

  // app initialization
  AppService := TAppService.Create(Application);

  Application.CreateForm(TMainForm, MainForm);
  Application.CreateForm(TedMessage, edMessage);
  Application.Run;
end.
