CREATE SEQUENCE GEN_Franchises_ID;

SET TERM ^ ;

create trigger Franchises_bi for Franchises
active before insert position 0
as
begin
  if (new.nummer is null) then
    new.nummer = gen_id(gen_Franchises_id,1);
end^

SET TERM ; ^

