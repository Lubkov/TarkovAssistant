unit ME.Frame.Quest;
//
interface

uses
  System.SysUtils, System.Types, System.UITypes, System.Classes, System.Variants, 
  FMX.Types, FMX.Graphics, FMX.Controls, FMX.Forms, FMX.Dialogs, FMX.StdCtrls,
  System.Rtti, FMX.Grid.Style, System.ImageList, FMX.ImgList, System.Actions,
  FMX.ActnList, FMX.Grid, FMX.ScrollBox, FMX.Controls.Presentation,
  ME.DB.Quest, Data.DB, MemDS, DBAccess, Uni, Fmx.Bind.Grid,
  System.Bindings.Outputs, Fmx.Bind.Editors, Data.Bind.EngExt,
  Fmx.Bind.DBEngExt, Data.Bind.Components, Data.Bind.Grid, Data.Bind.DBScope,
  FMX.Edit, ME.Trader, FMX.Layouts, ME.Filter.Trader, ME.Filter.Map;

type
  TfrQuest = class(TFrame)
    paTopPanel: TPanel;
    edAddQuest: TSpeedButton;
    edEditQuest: TSpeedButton;
    edDeleteQuest: TSpeedButton;
    laTitle: TLabel;
    ActionList1: TActionList;
    acAddQuest: TAction;
    acEditQuest: TAction;
    acDeleteQuest: TAction;
    ImageList1: TImageList;
    F: TUniQuery;
    FID: TIntegerField;
    FName: TWideStringField;
    FTrader: TIntegerField;
    FTraderName: TWideStringField;
    BindSourceDB1: TBindSourceDB;
    Grid: TStringGrid;
    LinkGridToDataSourceBindSourceDB1: TLinkGridToDataSource;
    BindingsList1: TBindingsList;
    edFilterText: TEdit;
    FilterLayout: TLayout;
    TraderLayout: TLayout;
    MapLayout: TLayout;
    procedure ActionList1Update(Action: TBasicAction; var Handled: Boolean);
    procedure acAddQuestExecute(Sender: TObject);
    procedure acEditQuestExecute(Sender: TObject);
    procedure acDeleteQuestExecute(Sender: TObject);
    procedure FCalcFields(DataSet: TDataSet);
    procedure BindSourceDB1SubDataSourceDataChange(Sender: TObject; Field: TField);
    procedure GridCellDblClick(const Column: TColumn; const Row: Integer);
  private
    FQuestID: Variant;
    FOnQuestChanged: TQuestChangedEvent;
    FTraderFilter: TTraderFilter;
    FMapFilter: TMapFilter;

    procedure OnApplyFilter(Sender: TObject);
    function InternalQuestEdit(const Quest: TDBQuest): Boolean;
    procedure QuestEdit(const Index: Integer);
  public
    constructor Create(AOwner: TComponent); override;
    destructor Destroy; override;

    procedure Init;

    property OnQuestChanged: TQuestChangedEvent read FOnQuestChanged write FOnQuestChanged;
  end;

implementation

{$R *.fmx}

uses
  App.Service, ME.DB.Utils, ME.Presenter.Quest, ME.Edit.Quest, ME.Dialog.Message,
  ME.Service.Quest;

{ TfrQuest }

constructor TfrQuest.Create(AOwner: TComponent);
begin
  inherited;

  FQuestID := Null;
  Grid.RowCount := 0;
  FOnQuestChanged := nil;
  F.FilterOptions := F.FilterOptions + [TFilterOption.foCaseInsensitive];

  FTraderFilter := TTraderFilter.Create(Self);
  FTraderFilter.Parent := TraderLayout;
  FTraderFilter.Align := TAlignLayout.Client;
  FTraderFilter.OnChange := OnApplyFilter;
  FTraderFilter.Init;

  FMapFilter := TMapFilter.Create(Self);
  FMapFilter.Parent := MapLayout;
  FMapFilter.Align := TAlignLayout.Client;
  FMapFilter.OnChange := OnApplyFilter;
  FMapFilter.EditItem.Visible := False;

  edFilterText.OnChangeTracking := OnApplyFilter;
end;

destructor TfrQuest.Destroy;
begin
  FOnQuestChanged := nil;

  inherited;
end;

procedure TfrQuest.Init;
begin
  F.Close;
  F.Connection := AppService.DBConnection.Connection;
  F.SQL.Text := 'SELECT ' + TDBQuest.FieldList + ' FROM ' + TDBQuest.EntityName;
  F.SQL.Add('ORDER BY Name');
  F.Open;

  FMapFilter.Init;
  FMapFilter.KeyValue := Null;
  FMapFilter.OnChange := OnApplyFilter;
end;

procedure TfrQuest.FCalcFields(DataSet: TDataSet);
begin
  FTraderName.AsString := TraderToStr(TTrader(FTrader.AsInteger));
end;

procedure TfrQuest.BindSourceDB1SubDataSourceDataChange(Sender: TObject; Field: TField);
begin
  if FQuestID = FID.Value then
    Exit;

  FQuestID := FID.Value;
  if Assigned(FOnQuestChanged) then
    FOnQuestChanged(FID.Value, FMapFilter.KeyValue);
end;

procedure TfrQuest.OnApplyFilter(Sender: TObject);
var
  Filter: string;
  Key: Variant;
begin
  Key := FID.Value;

  F.DisableControls;
  try
    F.Close;
    F.SQL.Text :=
      ' SELECT ' +
      '       q.ID as ID, ' +
      '       q.Name as Name, ' +
      '       q.Trader as Trader ' +
      ' FROM Quest q ';

    Filter := '';
    if FTraderFilter.KeyValue <> Null then
      Filter := '(q.Trader = ' + VarToStr(FTraderFilter.KeyValue) + ')';

    if FMapFilter.KeyValue <> Null then begin
      F.SQL.Add('INNER JOIN Marker m ON (m.QuestID = q.ID) AND (m.MapID = ' + VarToStr(FMapFilter.KeyValue) + ')');

      if Filter <> '' then
        F.SQL.Add(' AND ' + Filter);
    end
    else
      if Filter <> '' then
        F.SQL.Add(' WHERE ' + Filter);

    F.SQL.Add('GROUP BY q.ID, q.Name, q.Trader');
    F.SQL.Add('ORDER BY q.Name');
    F.Open;

    // workaround - case sensitivity of the LIKE operator
    Filter := Trim(edFilterText.Text);
    if Filter <> '' then
      Filter := '(Name like ' + QuotedStr('%' + Filter + '%') + ')';

    F.Filter := Filter;
    F.Filtered := F.Filter <> '';
  finally
    F.EnableControls;
  end;

  if not IsNullID(Key) then
    F.Locate('ID', Key, []);

  if Assigned(FOnQuestChanged) then
    FOnQuestChanged(FID.Value, FMapFilter.KeyValue);
end;

function TfrQuest.InternalQuestEdit(const Quest: TDBQuest): Boolean;
var
  Presenter: TEditQuestPresenter;
  Dialog: TedQuest;
begin
  Dialog := TedQuest.Create(Self);
  try
    Presenter := TEditQuestPresenter.Create(Dialog, Quest);
    try
      Result := Presenter.Edit;
    finally
      Presenter.Free;
    end;
  finally
    Dialog.Free;
  end;
end;

procedure TfrQuest.QuestEdit(const Index: Integer);
var
  Quest: TDBQuest;
begin
  Quest := TDBQuest.Create;
  try
    if not QuestService.GetAt(FID.Value, Quest) then
      Exit;

    if InternalQuestEdit(Quest) then
      F.RefreshRecord;
  finally
    Quest.Free;
  end;
end;

procedure TfrQuest.acAddQuestExecute(Sender: TObject);
var
  Quest: TDBQuest;
begin
  Quest := TDBQuest.Create;
  try
    if not InternalQuestEdit(Quest) then
      Exit;

    F.DisableControls;
    try
      F.Refresh;
      F.Last;
    finally
      F.EnableControls;
    end;
  finally
    Quest.Free;
  end;
end;

procedure TfrQuest.acEditQuestExecute(Sender: TObject);
begin
  QuestEdit(FID.Value);
end;

procedure TfrQuest.acDeleteQuestExecute(Sender: TObject);
var
  Quest: TDBQuest;
  Presenter: TDelQuestPresenter;
  Dialog: TedMessage;
begin
  if IsNullID(FID.Value) then
    Exit;

  Quest := TDBQuest.Create;
  try
    Quest.ID := FID.Value;
    Quest.Name := FName.AsString;

    Dialog := TedMessage.Create(Self);
    try
      Presenter := TDelQuestPresenter.Create(Dialog, Quest);
      try
        if not Presenter.Delete then
          Exit;

        F.DisableControls;
        try
          F.Refresh;
        finally
          F.EnableControls;
        end;
      finally
        Presenter.Free;
      end;
    finally
      Dialog.Free;
    end;
  finally
    Quest.Free;
  end;
end;

procedure TfrQuest.GridCellDblClick(const Column: TColumn; const Row: Integer);
begin
  if not IsNullID(FID.Value) then
    QuestEdit(FID.Value);
end;

procedure TfrQuest.ActionList1Update(Action: TBasicAction; var Handled: Boolean);
begin
  acAddQuest.Enabled := True;
  acEditQuest.Enabled := acAddQuest.Enabled and not IsNullID(FID.Value);
  acDeleteQuest.Enabled := acAddQuest.Enabled and not IsNullID(FID.Value);
end;

end.
