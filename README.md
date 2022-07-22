# PhotoTools.Net

[![.NET build](https://github.com/hajduakos/PhotoToolsDotNet/actions/workflows/dotnetcore.yml/badge.svg)](https://github.com/hajduakos/PhotoToolsDotNet/actions/workflows/dotnetcore.yml)
[![Coverage Status](https://coveralls.io/repos/github/hajduakos/PhotoToolsDotNet/badge.svg?branch=master)](https://coveralls.io/github/hajduakos/PhotoToolsDotNet?branch=master)

Tools for processing photos.

## Building

- Dependencies on Linux: `sudo apt install libc6-dev libgdiplus`
- Dependencies on Mac: `brew install mono-libgdiplus`
  - `sudo ln -s /opt/homebrew/opt/mono-libgdiplus/lib/libgdiplus.dylib /usr/local/lib/` might also be needed
- Build: `dotnet build -c release`

## Testing

- Run tests: `dotnet test -c release`
