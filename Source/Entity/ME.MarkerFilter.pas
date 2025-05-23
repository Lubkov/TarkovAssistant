unit ME.MarkerFilter;

interface

uses
  System.SysUtils, System.Classes, System.Variants,
  ME.DB.Map, ME.DB.Marker;

type
  TBoolArray = array of Boolean;
  TMapChangedEvent = procedure(Map: TDBMap) of object;

  TMarkerFilter = class
  private
    FGroupFilter: TMarkerKindSet;
    FQuestFilter: TBoolArray;
    FUpdateCount: Integer;
    FOnChanged: TNotifyEvent;
    FOnMapChanged: TMapChangedEvent;

    procedure DoChange;
    procedure SelectAllQuest(const Enable: Boolean);
    function GetUpdating: Boolean;
  public
    constructor Create;
    destructor Destroy; override;

    procedure Clear;
    procedure Init(const Map: TDBMap);

    procedure EnableGroup(const Kind: TMarkerKind);
    procedure DisableGroup(const Kind: TMarkerKind);
    procedure EnableQuest(const Index: Integer);
    procedure DisablQuest(const Index: Integer);
    procedure EnableAll;
    procedure DisableAll;

    function IsGropupEnable(const Kind: TMarkerKind): Boolean;
    function IsQuestEnable(const Index: Integer): Boolean;

    procedure BeginUpdate;
    procedure EndUpdate;

    // read-only filter state
    property GroupFilter: TMarkerKindSet read FGroupFilter;
    property QuestFilter: TBoolArray read FQuestFilter;

    property Updating: Boolean read GetUpdating;

    // events
    property OnChanged: TNotifyEvent read FOnChanged write FOnChanged;
    property OnMapChanged: TMapChangedEvent read FOnMapChanged write FOnMapChanged;
  end;

implementation

{ TMarkerFilter }

constructor TMarkerFilter.Create;
begin
  inherited;

  FUpdateCount := 0;
  FOnChanged := nil;
  FOnMapChanged := nil;
  Clear;
end;

destructor TMarkerFilter.Destroy;
begin
  FOnChanged := nil;
  FOnMapChanged := nil;

  inherited;
end;

procedure TMarkerFilter.DoChange;
begin
  if Assigned(FOnChanged) and not Updating then
    FOnChanged(Self);
end;

procedure TMarkerFilter.SelectAllQuest(const Enable: Boolean);
begin
  if Length(QuestFilter) > 0 then
    FillChar(QuestFilter[0], Length(QuestFilter) * SizeOf(Boolean), Enable.ToInteger);
end;

procedure TMarkerFilter.Clear;
begin
  FGroupFilter := [];
  SetLength(FQuestFilter, 0);
end;

procedure TMarkerFilter.Init(const Map: TDBMap);
begin
  SetLength(FQuestFilter, Map.Quests.Count);
  SelectAllQuest(False);

  if Assigned(FOnMapChanged) then
    FOnMapChanged(Map);
end;

procedure TMarkerFilter.EnableGroup(const Kind: TMarkerKind);
begin
  FGroupFilter := FGroupFilter + [Kind];
  DoChange;
end;

procedure TMarkerFilter.DisableGroup(const Kind: TMarkerKind);
begin
  FGroupFilter := FGroupFilter - [Kind];
  DoChange;
end;

procedure TMarkerFilter.EnableQuest(const Index: Integer);
begin
  FQuestFilter[Index] := True;
  DoChange;
end;

procedure TMarkerFilter.DisablQuest(const Index: Integer);
begin
  FQuestFilter[Index] := False;
  DoChange;
end;

procedure TMarkerFilter.EnableAll;
begin
  SelectAllQuest(True);
  FGroupFilter := [TMarkerKind.PMCExtraction, TMarkerKind.ScavExtraction, TMarkerKind.CoopExtraction];
  DoChange;
end;

procedure TMarkerFilter.DisableAll;
begin
  SelectAllQuest(False);
  FGroupFilter := [];
  DoChange;
end;

function TMarkerFilter.IsGropupEnable(const Kind: TMarkerKind): Boolean;
begin
  Result := Kind in FGroupFilter;
end;

function TMarkerFilter.IsQuestEnable(const Index: Integer): Boolean;
begin
  Result := FQuestFilter[Index];
end;

procedure TMarkerFilter.BeginUpdate;
begin
  Inc(FUpdateCount);
end;

procedure TMarkerFilter.EndUpdate;
begin
  Dec(FUpdateCount);
end;

function TMarkerFilter.GetUpdating: Boolean;
begin
  Result := FUpdateCount > 0;
end;

end.
