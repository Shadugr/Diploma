const editor = document.getElementById("editorPageEditor");
const boldButton = document.getElementById("boldButton");
const italicButton = document.getElementById("italicButton");
const underlineButton = document.getElementById("underlineButton");
const ulButton = document.getElementById("ulButton");
const olButton = document.getElementById("olButton");
const linkAddButton = document.getElementById("linkAddButton");
const imageInput = document.getElementById("imageInput");

document.addEventListener("load", changeSelect());
document.addEventListener("load", editor.focus());

function performAction(command) {
    document.execCommand(command, false, null);
    editor.focus();
}
function toggleLinkAdd() {
    var elem = document.getElementById("linkAdd");
    elem.classList.toggle("display");
}
function getContent() {
    document.getElementById("editorPageContent").value = document.getElementById("editorPageEditor").innerHTML;
}
function changeSelect() {
    var select = document.getElementById("selectType");
    var value = select.options[select.selectedIndex].value;
    if (value == "page") {
        document.getElementById("contentEditor").classList.remove("hidden");
        document.getElementById("contentEditor").classList.add("display");
        document.getElementById("pageContentLink").classList.add("hidden");
        document.getElementById("pageContentLink").classList.remove("display");
    }
    else if (value == "link") {
        document.getElementById("contentEditor").classList.add("hidden");
        document.getElementById("contentEditor").classList.remove("display");
        document.getElementById("pageContentLink").classList.remove("hidden");
        document.getElementById("pageContentLink").classList.add("display");
    }
    else if (value == "list") {
        document.getElementById("contentEditor").classList.add("hidden");
        document.getElementById("contentEditor").classList.remove("display");
        document.getElementById("pageContentLink").classList.add("hidden");
        document.getElementById("pageContentLink").classList.remove("display");
    }
}

boldButton.addEventListener("click", function () {
    performAction("bold");
});
italicButton.addEventListener("click", function () {
    performAction("italic");
});
underlineButton.addEventListener("click", function () {
    performAction("underline");
});
ulButton.addEventListener("click", function () {
    performAction("insertunorderedlist");
});
olButton.addEventListener("click", function () {
    performAction("insertorderedlist");
});
linkAddButton.addEventListener("click", function () {
    document.execCommand("createlink", false, document.getElementById("linkAddInput").value);
    editor.focus();
    document.getElementById("linkAddInput").value = "";
    toggleLinkAdd();
});
editor.addEventListener("keypress", function () {
    if (this.innerHTML == "" || this.innerHTML == "<br>") {
        this.innerHTML = "<div></br></div>";
    }
})
imageInput.addEventListener("change", ev => {
    const selection = window.getSelection();
    const range = document.createRange();
    selection.removeAllRanges();
    range.selectNodeContents(editor);
    range.collapse(false);
    selection.addRange(range);
    editor.focus();
    const formdata = new FormData()
    formdata.append("UploadImage", ev.target.files[0])
    jQuery.ajax({
        type: 'POST',
        url: "/Maineditor?handler=Uploader",
        data: formdata,
        cache: false,
        contentType: false,
        processData: false,
        success: function (repo) {
            document.execCommand('insertImage', false, repo.status);
            editor.focus();
        },
        error: function () {
            alert("Error occurs");
        }
    });
})