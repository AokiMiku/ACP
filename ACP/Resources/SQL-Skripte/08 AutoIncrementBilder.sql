CREATE SEQUENCE GEN_Bilder_ID;

SET TERM ^ ;

create trigger Bilder_bi for Bilder
active before insert position 0
as
begin
  if (new.nummer is null) then
    new.nummer = gen_id(gen_Bilder_id,1);
end^

SET TERM ; ^

