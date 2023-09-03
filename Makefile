## migration name=$1: creates new migration with given name
.PHONY: migration
migration:
	dotnet ef migrations add ${name}

## migrate: migrated database
.PHONY: migrate
migrate:
	dotnet ef database update

## run: starts application
.PHONY: run
run:
	dotnet run --launch-profile https