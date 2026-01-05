using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VeloxDev.Core.MVVM;
using VeloxDev.Wiki.ViewModels.WikiCOMs.Interfaces;

namespace VeloxDev.Wiki.ViewModels.WikiCOMs
{
    public partial class WikiPageViewModel
    {
        [VeloxProperty] private string _versions = string.Empty;
        [VeloxProperty] private ObservableCollection<IWikiPageElement> _elements = [];

        [VeloxCommand] 
        private Task AddElement(object element, CancellationToken ct)
        {
            if (element is not IWikiPageElement newElement) 
                return Task.CompletedTask;

            _elements.Add(newElement);
            newElement.Parent = this;
            RefreshElementIndices();
            
            return Task.CompletedTask;
        }

        [VeloxCommand] 
        private Task RemoveElement(object element, CancellationToken ct)
        {
            if (element is not IWikiPageElement elementToRemove) 
                return Task.CompletedTask;

            if (_elements.Remove(elementToRemove))
            {
                elementToRemove.Parent = null;
                elementToRemove.Index = -1;
                RefreshElementIndices();
            }
            
            return Task.CompletedTask;
        }

        [VeloxCommand]
        private Task SortElements(object sortKey, CancellationToken ct)
        {
            if (sortKey is not Func<IWikiPageElement, IComparable> keySelector)
                return Task.CompletedTask;

            var sortedList = _elements.OrderBy(keySelector).ToList();
            _elements.Clear();
            
            foreach (var item in sortedList)
            {
                _elements.Add(item);
            }
            
            RefreshElementIndices();
            return Task.CompletedTask;
        }

        private void RefreshElementIndices()
        {
            for (int i = 0; i < _elements.Count; i++)
            {
                if (_elements[i] is IWikiPageElement element)
                {
                    element.Index = i;
                }
            }
        }
    }
}