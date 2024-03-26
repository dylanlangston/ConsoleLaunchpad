SHELL=/bin/bash

help: ## Display the help menu.
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[36m%-30s\033[0m %s\n", $$1, $$2}'

build: ## Build entire project
	@if [ -n "$$AndroidSdkDirectory" ]; then \
		cd ./src; dotnet build; \
	else \
		cd ./src; dotnet build /p:AndroidSdkDirectory=$$HOME/Android/Sdk; \
	fi

run-desktop: ## Run desktop version
	@cd ./src/ConsoleLaunchpad.Desktop; dotnet run

run-android: ## Run android version
	@if [ -n "$$AndroidSdkDirectory" ]; then \
		cd ./src/ConsoleLaunchpad.Android; dotnet build -t:Run -f net8.0-android; \
	else \
		cd ./src/ConsoleLaunchpad.Android; dotnet build -t:Run -f net8.0-android /p:AndroidSdkDirectory=$$HOME/Android/Sdk; \
	fi

run-web: ## Run browser version
	@cd ./src/ConsoleLaunchpad.Browser; dotnet run

develop-web: ## Develop browser version
	@cd ./src/ConsoleLaunchpad.Browser; dotnet watch

setup: ## Setup local environment
	@cd ./src;dotnet workload restore
	@cd ./src;dotnet restore

clean: ## Clean local environment
	@cd ./src;dotnet clean