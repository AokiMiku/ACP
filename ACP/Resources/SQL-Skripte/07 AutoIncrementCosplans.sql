CREATE SEQUENCE GEN_Cosplans_ID;

SET TERM ^ ;

create trigger Cosplans_bi for Cosplans
active before insert position 0
as
begin
  if (new.nummer is null) then
    new.nummer = gen_id(gen_Cosplans_id,1);
end^

SET TERM ; ^

