(function () {
    let basePath = "";

    // Extract the base path (e.g., /SKJBilling) if any
    const segments = window.location.pathname.split('/');
    if (segments.length > 1 && segments[1]) {
        basePath = '/' + segments[1];
    }

    // Apply to all relative AJAX URLs
    $.ajaxPrefilter(function (options, originalOptions, jqXHR) {
        // Only change relative URLs (not full http/https URLs)
        if (!/^https?:\/\//i.test(options.url)) {
            if (!options.url.startsWith(basePath)) {
                options.url = basePath + options.url;
            }
        }
    });
})();
