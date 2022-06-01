let loadingElement = {
    loadingContainer: document.querySelector('body .loading-container'),
    hide: function () { this.loadingContainer.style.display = 'none'; },
    show: function () { this.loadingContainer.style.display = 'flex'; }
};