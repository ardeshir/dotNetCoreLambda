using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

using Amazon.CodeDeploy;
using Amazon.CodeDeploy.Model;

using Amazon.Lambda.Core;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace lambda 
{
    public class CodeDeployInvokeRequest
    {
        public string EnvironmentId { get; set;}
        public string ApplicationName {get; set;}
        public string DeploymentGroup {get; set;}
        public string S3BucketName {get; set;}
        public string S3KeyName {get; set;}
    }

    public class Function {
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>

        public async Task<string> FunctionHandler(CodeDeployInvokeRequest input, ILambdaContext context)
        {
             String envId = input.EnvironmentId;
             String appName  = input.ApplicationName;

            AmazonCodeDeployClient client = new AmazonCodeDeployClient();

            var request = new CreateDeploymentRequest();

            request.ApplicationName = appName;
            request.DeploymentGroupName = input.DeploymentGroup;
            request.Description = "Testing Deployment created by Lambda";
            request.Revision = new RevisionLocation()
            {
                RevisionType = Amazon.CodeDeploy.RevisionLocationType.S3,
                S3Location = new S3Location()
                {
                    Bucket = input.S3BucketName,
                    BucketType = Amazon.CodeDeploy.BundleType.Zip,
                    Key = input.S3KeyName 
                }
            };

            var response = await client.CreateDeploymentAsync(request);
            return response.ToString();
        }
        
    }
}