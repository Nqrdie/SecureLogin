#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

cd "$ROOT_DIR/API"
dotnet build

cd "$ROOT_DIR/Website"
npm run build

cd "$ROOT_DIR/API"
dotnet run &
API_PID=$!

cd "$ROOT_DIR/Website"
node server.js &
FRONTEND_PID=$!

cleanup() {
  kill "$API_PID" "$FRONTEND_PID" 2>/dev/null || true
}
trap cleanup EXIT INT TERM

wait
