#!/usr/bin/env bash
dotnet run --project src/LoadTestToolbox.csproj -c Release hammer $@
