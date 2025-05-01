-- public.vehicles definition

-- Drop table

-- DROP TABLE public.vehicles;

CREATE TABLE public.vehicles (
	id serial4 NOT NULL,
	vin varchar(17) NOT NULL,
	make varchar(50) NULL,
	model varchar(50) NULL,
	"year" int4 NULL,
	title_state varchar(2) NULL,
	registration_status varchar(20) NULL,
	emissions_status varchar(10) NULL,
	recall_status varchar(50) NULL,
	created_at timestamp DEFAULT now() NULL,
	CONSTRAINT vehicles_pkey PRIMARY KEY (id),
	CONSTRAINT vehicles_vin_key UNIQUE (vin)
);
CREATE INDEX idx_vehicles_vin ON public.vehicles USING btree (vin);