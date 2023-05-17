create table if not exists public.movie (
	id bigint primary key generated always as identity,
	name varchar(64) not null,
	synopsis varchar(512),
	duration decimal not null,
	genre smallint not null
);

create table if not exists public.session (
	id bigint primary key generated always as identity,
	movie_id bigint not null,
	date timestamptz not null,
	CONSTRAINT fk_movie FOREIGN KEY (movie_id) REFERENCES movie (id)
);
