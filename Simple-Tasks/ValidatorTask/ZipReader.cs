using Ria;
using Ionic.Zip;
using Exy;
using Arvy;

namespace ValidatorTask;

public class ZipReader {
    public void ListOutZipContents(PipelineContext context) {
        try {
            ZipReaderConfiguration config = new ZipReaderConfigLoader().Load();
            var nameAndPath = new List<KeyValuePair<String, String>>();
            foreach (ZipReaderPath zipPath in config.Paths) {
                var stream = (Stream) File.Open(zipPath.Path, FileMode.Open, FileAccess.Read, FileShare.Read);
                using (var zip = ZipFile.Read(stream)) {
                    foreach (ZipEntry entry in zip) {
                        if (Path.GetExtension(entry.FileName).Equals(".zip", StringComparison.InvariantCultureIgnoreCase)) {
                            String internalZipFilePath = entry.FileName + "/";

                            var contentStream = new MemoryStream();
                            entry.Extract(contentStream);
                            contentStream.Position = 0;
                            using (var internalZip = ZipFile.Read(contentStream))
                                foreach (ZipEntry internalZipEntry in internalZip)
                                    if (!internalZipEntry.FileName.EndsWith("/")) {
                                        nameAndPath.Add(new KeyValuePair<String, String>(Path.GetFileName(internalZipEntry.FileName), internalZipFilePath + internalZipEntry.FileName));
                                    }
                        }
                        else
                            if (!entry.FileName.EndsWith("/"))
                                nameAndPath.Add(new KeyValuePair<String, String>(Path.GetFileName(entry.FileName), entry.FileName));
                    }
                }
            }

            context.Data["NameAndPath"] = nameAndPath;
        }
        catch (Exception ex) {
            context.ActionMessages.Add(new ActionResponseViewModel(ActionResponseViewModel.Error, ex.GetExceptionMessage()));
            context.Cancelled = true;
        }
    }
}