SHELL=/bin/bash

help: ## Display the help menu.
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[36m%-30s\033[0m %s\n", $$1, $$2}'

build: ## Build entire project
	@cd ./src; dotnet build

run-desktop: ## Run desktop version
	@cd ./src/ConsoleLaunchpad.Desktop; dotnet run

setup: ## Setup local environment
	@cd ./src
	@dotnet workload restore
	@dotnet restore

clean: ## Clean local environment
	@cd ./src
	@dotnet clean