# HubWebApp

This folder contains a simple webapp that has a single SignalR hub with a simple "Marco" hub method that reflexively transmits "Polo".

(with one argument of a byte array payload)

# consolerepro

This folder has a SignalR client console app that does some minor spamming of Marco-Polo on the hub and exits.

**NOTE: Line 12 needs to have `[CHANGE THIS TO YOUR WEBAPP URL]` filled-in.**

For reproducing the crash, it has been built via:

`dotnet publish -r linux-arm  -p:PublishSingleFile=true --self-contained true -c Release`

Running on a 32-bit arm linux system it has been observed to segfault some of the time, when first establishing a SignalR connection.

The `repro.sh` file is a bash helper script that repeatedly runs the console app in a loop until the first crash.
