create table if not exists public.user (
	id bigint primary key generated always as identity,
	login varchar(32) not null,
	password text not null,
	name varchar(32) not null,
	profile smallint not null
);

insert into public.user (login, password, name, profile) values
('Admin', '$2a$12$RpZ2ANlJOAFXuKh4m.y2muDUJSty8Nwb10FxdwNANJ23wHFiteut6', 'Admin', 1);