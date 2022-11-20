namespace TheBlog.Utilities
{
    /// <summary>
    ///  Validates the file the user tries to upload.
    ///  Checks if the file extension is accepted. (.png or .jpg).
    ///  Checks if the file signature matches the file extension.
    /// </summary>
    public class FileValidationService
    {
        // Signatures for accepted file types to upload
        private static readonly Dictionary<string, List<byte[]>> _fileSignatures = new Dictionary<string, List<byte[]>>
        {
            { ".png", new List<byte[]>
                 {
                     new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }
                 }
            },
            { ".jpg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 }
                }
            },
        };

        /// <summary>
        /// Validates the signature and extension of the file.
        /// </summary>
        /// <param name="uploadedFile">The <see cref="IFormFile"> object</param>
        /// <returns>
        /// Returns <see cref="true"> if the file is valid.
        /// Returns <see cref="false"> if the file is invalid.
        /// </returns>
        public bool IsValid(IFormFile uploadedFile)
        {
            // Get the file extension
            string ext = Path.GetExtension(uploadedFile.FileName);

            // Check if the there is a signature for the file extension
            if (_fileSignatures.ContainsKey(ext) == false)
            {
                // There is no signature for the file extension to compare with.
                // The file is invalid.
                return false;
            }

            // Create BinaryReader for file
            using (var reader = new BinaryReader(uploadedFile.OpenReadStream()))
            {
                // Get possible signatures for file type depending on file extension
                var signatures = _fileSignatures[ext];

                // Get the length of the longest signature for the file type.
                var numberOfBytesOfLongestSignature = signatures.Max(m => m.Length);

                // Read the number of bytes from the file as the longest signature
                var headerBytes = reader.ReadBytes(numberOfBytesOfLongestSignature);

                // Loop over all possible signatures for the file type
                foreach (var signature in signatures)
                {
                    // Get the number of bytes according to the signature length
                    var bytesToCompare = headerBytes.Take(signature.Length);

                    // Compare the bytes from the file with the bytes from the signature
                    if (bytesToCompare.SequenceEqual(signature))
                    {
                        // They match so the file valid
                        return true;
                    }
                }

                // No signature matched the file so the file is invalid.
                return false;
            }
        }
    }
}
