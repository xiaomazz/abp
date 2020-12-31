﻿using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.CmsKit.Pages
{
    public abstract class PageRepository_Test<TStartupModule> : CmsKitTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly CmsKitTestData _cmsKitTestData;
        private readonly IPageRepository _pageRepository;

        protected PageRepository_Test()
        {
            _cmsKitTestData = GetRequiredService<CmsKitTestData>();
            _pageRepository = GetRequiredService<IPageRepository>();
        }

        [Fact]
        public async Task CountAsync()
        {
            var totalCount = await _pageRepository.GetCountAsync();
            
            totalCount.ShouldBe(2);

            var filteredCount = await _pageRepository.GetCountAsync(_cmsKitTestData.Page_2_Title);
            
            filteredCount.ShouldBe(1);
        }
        
        [Fact]
        public async Task GetListAsync()
        {
            var list = await _pageRepository.GetListAsync();

            list.ShouldNotBeNull();
            list.Count.ShouldBe(2);

            var list_page_1 = await _pageRepository.GetListAsync(maxResultCount: 1);
            var list_page_2 = await _pageRepository.GetListAsync(maxResultCount: 1, skipCount: 1);
            
            list_page_1.ShouldNotBeNull();
            list_page_1.Count.ShouldBe(1);
            list_page_1.First().Title.ShouldBe(_cmsKitTestData.Page_1_Title);
            
            list_page_2.ShouldNotBeNull();
            list_page_1.Count.ShouldBe(1);
            list_page_2.First().Title.ShouldBe(_cmsKitTestData.Page_2_Title);
        }
        
        [Fact]
        public async Task ShouldGetByUrlAsync()
        {
            var page = await _pageRepository.GetByUrlAsync(_cmsKitTestData.Page_1_Url);

            page.ShouldNotBeNull();
            page.Description.ShouldBe(_cmsKitTestData.Page_1_Description);
        }
        
        [Fact]
        public async Task ShouldFindByUrlAsync()
        {
            var page = await _pageRepository.FindByUrlAsync(_cmsKitTestData.Page_1_Url);

            page.ShouldNotBeNull();
            page.Description.ShouldBe(_cmsKitTestData.Page_1_Description);
        }
        
        [Fact]
        public async Task ShouldNotFindByUrlAsync()
        {
            var page = await _pageRepository.FindByUrlAsync("not-exist-lyrics");

            page.ShouldBeNull();
        }
        
        [Fact]
        public async Task ShouldBeExistAsync()
        {
            var page = await _pageRepository.DoesExistAsync(_cmsKitTestData.Page_1_Url);

            page.ShouldBeTrue();
        }
        
        [Fact]
        public async Task ShouldNotBeExistAsync()
        {
            var page = await _pageRepository.DoesExistAsync("not-exist-lyrics");

            page.ShouldBeFalse();
        }
    }
}