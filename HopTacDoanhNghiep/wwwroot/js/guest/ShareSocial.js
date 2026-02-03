function openShare(url) {
    window.open(url, '_blank', 'width=600,height=500');
}

function shareFacebook(url) {
    openShare(`https://www.facebook.com/sharer/sharer.php?u=${encodeURIComponent(url)}`);
}

function shareZalo(url) {
    openShare(`https://zalo.me/share?url=${encodeURIComponent(url)}`);
}

function shareLinkedIn(url) {
    openShare(`https://www.linkedin.com/sharing/share-offsite/?url=${encodeURIComponent(url)}`);
}
