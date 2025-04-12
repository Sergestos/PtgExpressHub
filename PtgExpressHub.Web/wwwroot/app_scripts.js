function downloadFileFromStream(filename, contentType, data) {

    const file = new File([data], filename, { type: contentType });
    const exportUrl = URL.createObjectURL(file);

    const a = document.createElement("a");
    document.body.appendChild(a);
    a.href = exportUrl;
    a.download = filename;
    a.target = "_self";
    a.click();

    URL.revokeObjectURL(exportUrl);
    a.remove();
}

window.checkAndHideExitBtn = () => {
    var currentUrl = window.location.pathname; 
    if (currentUrl === '/auth/login') {  
        var elements = document.getElementsByClassName('hide-mark');
        for (var i = 0; i < elements.length; i++) {
            elements[i].style.display = 'none';
        }
    }
};