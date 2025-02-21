using CMSDemo.Migration.Models;
using CMSDemo.Models.Media;
using CMSDemo.Models.Pages;
using EPiServer.DataAccess;
using EPiServer.Framework.Blobs;
using EPiServer.Security;
using EPiServer.Web;

namespace CMSDemo.Migration.Services
{
    public interface IContentMigrationService
    {
        Task MigrateContentAsync(IEnumerable<ContentItem> contentItems);
        Task<bool> DeleteContentAsync(int contentId);
    }

    /// <summary>
    /// 
    /// </summary>
    public class ContentMigrationService : IContentMigrationService
    {
        private readonly IContentRepository _contentRepository;
        private readonly IContentLoader _contentLoader;
        private readonly IBlobFactory _blobFactory;

        public ContentMigrationService(IContentRepository contentRepository, IContentLoader contentLoader, IBlobFactory blobFactory)
        {
            _contentRepository = contentRepository;
            _contentLoader = contentLoader;
            _blobFactory = blobFactory;
        }

        public async Task MigrateContentAsync(IEnumerable<ContentItem> contentItems)
        {
            foreach (var item in contentItems)
            {
                PageReference parentReference = item.ParentContentId > 0
                    ? new PageReference(item.ParentContentId)
                    : PageReference.RootPage;

                StandardPage existingPage = null;

                if (item.ContentId > 0)
                {
                    _contentRepository.TryGet(new ContentReference(item.ContentId), out existingPage);
                }

                ContentReference teaserImageRef = ContentReference.EmptyReference;

                if (!string.IsNullOrEmpty(item.TeaserImageUrl))
                {
                    teaserImageRef = await UploadImageFromUrlAsync(item.TeaserImageUrl);
                }

                if (existingPage != null)
                {
                    var writableClone = existingPage.CreateWritableClone() as StandardPage;
                    if (writableClone != null)
                    {
                        writableClone.Name = item.Name;
                        writableClone.MainBody = new XhtmlString(item.ContentData);
                        writableClone.PageImage = teaserImageRef; // Set image reference

                        _contentRepository.Save(writableClone, SaveAction.Publish, AccessLevel.NoAccess);
                    }
                }
                else
                {
                    var newContent = _contentRepository.GetDefault<StandardPage>(parentReference);
                    newContent.Name = item.Name;
                    newContent.MainBody = new XhtmlString(item.ContentData);
                    newContent.PageImage = teaserImageRef; // Set image reference

                    _contentRepository.Save(newContent, SaveAction.Publish, AccessLevel.NoAccess);
                }
            }

            await Task.CompletedTask;
        }

        private async Task<ContentReference> UploadImageFromUrlAsync(string imageUrl)
        {
            using HttpClient client = new HttpClient();
            byte[] imageBytes = await client.GetByteArrayAsync(imageUrl);

            var mediaFolder = _contentRepository.GetDefault<ImageFile>(SiteDefinition.Current.GlobalAssetsRoot);
            mediaFolder.Name = Path.GetFileName(imageUrl);

            var blob = _blobFactory.CreateBlob(mediaFolder.BinaryDataContainer, ".jpg");
            using (var stream = new MemoryStream(imageBytes))
            {
                blob.Write(stream);
            }

            mediaFolder.BinaryData = blob;
            return _contentRepository.Save(mediaFolder, SaveAction.Publish, AccessLevel.NoAccess);
        }

        /// <summary>
        /// Delete content by ID
        /// </summary>
        public async Task<bool> DeleteContentAsync(int contentId)
        {
            if (contentId <= 0)
            {
                return false; // Invalid ID
            }

            if (_contentRepository.TryGet(new ContentReference(contentId), out IContent contentToDelete))
            {
                _contentRepository.Delete(contentToDelete.ContentLink, true, AccessLevel.NoAccess);
                return true;
            }

            return false; // Content not found
        }
    }
}
