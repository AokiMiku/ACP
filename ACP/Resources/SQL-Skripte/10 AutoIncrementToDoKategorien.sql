CREATE SEQUENCE GEN_ToDoKategorien_ID;

SET TERM ^ ;

create trigger ToDoKategorien_bi for ToDoKategorien
active before insert position 0
as
begin
  if (new.nummer is null) then
    new.nummer = gen_id(GEN_ToDoKategorien_ID,1);
end^

SET TERM ; ^

