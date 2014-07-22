using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SemVerServer.Models;
using VersionProvider.Core;
using VersionProvider.Core.Persistence;

namespace SemVerServer.Controllers.Api
{
    public class SemVerController : ApiController
    {
        private readonly ISemanticVersionProviderFactory _semanticVersionProviderFactory;

        public SemVerController()
        {
            _semanticVersionProviderFactory = new SemanticVersionProviderFactory();
        }

        [HttpGet]
        public SemanticVersionInfo GetCurrentVersion(string scope)
        {
            SemanticVersionInfo result = null;
            Execute(scope, provider => result = provider.GetCurrentVersion());
            return result;
        }

        [HttpGet]
        public SemanticVersionInfo GetNextPreRelaseVersion(string scope, string preReleaseIdentifier = null, string buildMetadata = null)
        {
            SemanticVersionInfo result = null;
            Execute(scope, provider => result = provider.GetNextPreRelaseVersion(preReleaseIdentifier, buildMetadata));
            return result;
        }

        [HttpGet]
        public SemanticVersionInfo GetNextBugFixVersion(string scope, string preReleaseIdentifier = null, string buildMetadata = null)
        {
            SemanticVersionInfo result = null;
            Execute(scope, provider => result = provider.GetNextBugFixVersion(preReleaseIdentifier, buildMetadata));
            return result;
        }

        [HttpGet]
        public SemanticVersionInfo GetNextFeatureVersion(string scope, string preReleaseIdentifier = null, string buildMetadata = null)
        {
            SemanticVersionInfo result = null;
            Execute(scope, provider => result = provider.GetNextFeatureVersion(preReleaseIdentifier, buildMetadata));
            return result;
        }

        [HttpGet]
        public SemanticVersionInfo GetNextBreakingChangesVersion(string scope, string preReleaseIdentifier = null, string buildMetadata = null)
        {
            SemanticVersionInfo result = null;
            Execute(scope, provider => result = provider.GetNextBreakingChangesVersion(preReleaseIdentifier, buildMetadata));
            return result;
        }

        [HttpPost]
        public void SetVersion(string scope, SemanticVersionInfo versionInfo)
        {
            Execute(scope, provider => provider.SetVersion(versionInfo));
        }

        private void Execute(string scope, Action<ISemanticVersionProvider> action)
        {
            var provider = GetProvider(scope);
            try
            {
                action(provider);
            }
            catch (ConcurrentPersistenceException ex)
            {
                throw CreateResponseException("Concurrency error: " + ex.Message);
            }
            catch (InvalidVersionException ex)
            {
                throw CreateResponseException("Version validation error: " + ex.Message);
            }
            catch (Exception)
            {
                throw CreateResponseException("Unknown error.");
            }
        }

        private ISemanticVersionProvider GetProvider(string scope)
        {
            var result = _semanticVersionProviderFactory.CreateProvider(scope);
            if (result == null)
            {
                // no such provider
                throw CreateResponseException("No such scope configured.");
            }

            return result;
        }

        private HttpResponseException CreateResponseException(string message)
        {
            var httpError = new HttpError(message);
            var response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, httpError);
            throw new HttpResponseException(response);
        }
    }
}