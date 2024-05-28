var customBasePath = args.FirstOrDefault();
if (string.IsNullOrEmpty(customBasePath))
    customBasePath = Environment.CurrentDirectory;

var outDir = Directory.GetParent(customBasePath)?.Parent;
if (outDir is null)
{
    Console.WriteLine("You must be not in a root");
    return;
}

var outFile = Path.Combine(outDir.FullName, "files.txt");
if (File.Exists(outFile))
    File.Delete(outFile);

using var dumpFile = File.OpenWrite(outFile);
using var dump = new StreamWriter(dumpFile);
foreach (var file in Directory.EnumerateFiles(customBasePath, "*.*", SearchOption.AllDirectories))
{
    var relative = Path.GetRelativePath(customBasePath, Path.GetDirectoryName(file) ?? "")
        .Replace('\\', '/');
    if (relative.StartsWith('.'))
        relative = relative[1..];
    if (!relative.EndsWith('/'))
        relative += '/';

    dump.WriteLine($"\"{file}\" \"../../../{relative}\"");
}
dumpFile.Flush();
