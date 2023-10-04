create table if not exists public.user (
	id bigint primary key generated always as identity,
	login varchar(32) not null,
	password text not null,
	name varchar(32) not null,
	profile smallint not null
);
