using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections;
using System.Collections.Generic;

namespace IdentityServer4
{
    public static class Config
    {
        // 定义api范围
        public static IEnumerable<ApiScope> ApiScopes => new []
        {
            new ApiScope
            {
                Name="sample_api", // 范围名称，自定义
                DisplayName="Sample API" // 范围显示名称，自定义
            }
        };

        // 定义客户端认证方式
        public static IEnumerable<Client> Clients => new[]
        {
            // 客户端范围认证
            new Client
            {
                ClientId="sample_client", // 客户端id
                ClientSecrets =
                {
                    new Secret("sample_client_secret".Sha256()) // 客户端秘钥

                },
                AllowedGrantTypes=GrantTypes.ClientCredentials, // 授权类型为客户端
                AllowedScopes={ "sample_api" } // 设置该客户端允许访问的api范围
            },
            // 客户端资源拥有者认证
            new Client
            {
                ClientId="sample_pass_client", // 客户端id
                ClientSecrets =
                {
                    new Secret("sample_client_secret".Sha256()) // 客户端秘钥

                },
                AllowedGrantTypes=GrantTypes.ResourceOwnerPassword, // 授权类型为资源拥有者
                AllowedScopes={ "sample_api" } // 设置该客户端允许访问的api范围
            },
            // 基于OIDC客户端
            new Client
            {
               ClientId="sample_mvc_client",
               ClientName="Sample MVC Client",
               ClientSecrets=
                {
                    new Secret("sample_client_secret".Sha256())
                },
               AllowedGrantTypes=GrantTypes.Code,
               RedirectUris={ "http://localhost:4001/signin-oidc"},  // 登录成功之后的回调地址
               PostLogoutRedirectUris={ "http://localhost:4001/signout-callback-oidc" }, // 注销/登出之后的回调地址
               AllowedScopes={ 
                 IdentityServerConstants.StandardScopes.OpenId,
                 IdentityServerConstants.StandardScopes.Profile,
                "sample_api"  // 用于oidc认证成功之后访问项目API的范围api接口
                },
               RequireConsent=true // 是否需要用户同步，当用户登录的时候需要用户进行是否同意
            }
        };

        // 基于OIDC协议
        public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>
        { 
          new IdentityResources.OpenId(),
          new IdentityResources.Profile()
        };

        // 基于OIDC添加测试用户
        public static List<TestUser> Users => new List<TestUser>() { 
        
          new TestUser()
          {
              SubjectId="1",
              Username="admin",
              Password="123456777"
          }
        };


    }
}
