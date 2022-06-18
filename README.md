# PhotoTools.Net

Tools for processing photos.

## Building

- Dependencies on Linux: `sudo apt install libc6-dev libgdiplus`
- Dependencies on Mac: `brew install mono-libgdiplus`
  - `sudo ln -s /opt/homebrew/opt/mono-libgdiplus/lib/libgdiplus.dylib /usr/local/lib/` might also be needed
- Build: `dotnet build -c release`

## Testing

- Run tests: `dotnet test -c release`
