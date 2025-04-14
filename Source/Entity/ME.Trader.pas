unit ME.Trader;

interface

uses
  System.SysUtils;

type
  TTrader = (None, Prapor, Therapist, Skier, Peacemaker, Mechanic, Ragman, Jaeger, Fence, Lightkeeper);

  function TraderToStr(Value: TTrader): string;

implementation

function TraderToStr(Value: TTrader): string;
begin
  case Value of
    TTrader.Prapor:
      Result := '������';
    TTrader.Therapist:
      Result := '��������';
    TTrader.Skier:
      Result := '������';
    TTrader.Peacemaker:
      Result := '����������';
    TTrader.Mechanic:
      Result := '�������';
    TTrader.Ragman:
      Result := '�����������';
    TTrader.Jaeger:
      Result := '�����';
    TTrader.Fence:
      Result := '�������';
    TTrader.Lightkeeper:
      Result := '����������';
  else
    Result := '';
  end;
end;

end.
