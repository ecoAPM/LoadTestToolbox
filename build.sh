#!/usr/bin/env bash
set -e
npm install
dotnet restore
dotnet build -c Release
