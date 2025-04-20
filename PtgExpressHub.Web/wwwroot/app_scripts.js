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

function copyTextToClipboard(text) {
    navigator.clipboard.writeText(text)
        .then()
        .catch(err => console.error("Error copying text:", err));
};