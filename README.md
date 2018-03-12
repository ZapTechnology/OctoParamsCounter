# OctoParamsCounter
Small tool to count Octopus parameters in files, e.g. ARM templates, powershell scripts.

Command line options:

  -i, --input         Required. Input files to be processed. e.g. *.json or foo.json

  -d, --directory     (Default: .) Directory to search.

  -p, --pattern       (Default: (\#\{|OctopusParameters\[)([^\/\]\}]+)) Regex for Octopus parameters.

  -r, --recursive     (Default: true) Recursive search

  -s, --references    (Default: false) Show references

  -v, --verbose       (Default: false) Show verbose log

  --help              Display this help screen.

  --version           Display version information.
