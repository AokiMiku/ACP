CREATE SEQUENCE GEN_ToDos_ID;

SET TERM ^ ;

create trigger ToDos_bi for ToDos
active before insert position 0
as
begin
  if (new.nummer is null) then
    new.nummer = gen_id(GEN_ToDos_ID,1);
end^

SET TERM ; ^

