-- public.apilogs definition

-- Drop table

-- DROP TABLE public.apilogs;

CREATE TABLE public.apilogs (
	transactionid int8 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE) NOT NULL,
	logtype varchar NOT NULL,
	start_time timestamp NOT NULL,
	end_time timestamp NULL,
	duration float4 NULL,
	requestmethod varchar NOT NULL,
	requestpath varchar NOT NULL,
	responsestatuscode int4 NULL
);