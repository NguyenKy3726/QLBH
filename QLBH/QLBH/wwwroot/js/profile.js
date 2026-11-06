(() => {
    const panel = document.querySelector('.profile-upload');
    if (!panel) return;
    const input = panel.querySelector('#profile-avatar-file');
    const preview = panel.querySelector('#profile-avatar-preview');
    const placeholder = panel.querySelector('#profile-avatar-placeholder');
    const reset = panel.querySelector('#profile-avatar-reset');
    const error = panel.querySelector('#profile-avatar-error');
    const currentAvatar = panel.dataset.currentAvatar || '';
    let previewUrl = null;

    const releasePreview = () => {
        if (previewUrl) URL.revokeObjectURL(previewUrl);
        previewUrl = null;
    };
    const restore = () => {
        releasePreview();
        input.value = '';
        if (currentAvatar) preview.src = currentAvatar;
        else preview.removeAttribute('src');
        preview.hidden = !currentAvatar;
        placeholder.hidden = !!currentAvatar;
        reset.hidden = true;
        error.textContent = '';
    };
    input.addEventListener('change', () => {
        const file = input.files?.[0];
        if (!file) { restore(); return; }
        error.textContent = '';
        if (file.size > 5 * 1024 * 1024 || !/\.(jpe?g|png|webp)$/i.test(file.name)) {
            restore();
            error.textContent = 'Vui lòng chọn ảnh JPG, PNG hoặc WEBP, tối đa 5 MB.';
            return;
        }
        releasePreview();
        previewUrl = URL.createObjectURL(file);
        preview.src = previewUrl;
        preview.hidden = false;
        placeholder.hidden = true;
        reset.hidden = false;
    });
    reset.addEventListener('click', restore);
    preview.addEventListener('error', () => {
        if (!previewUrl) return;
        restore();
        error.textContent = 'Không thể xem trước ảnh này. Vui lòng chọn lại ảnh.';
    });
    window.addEventListener('pagehide', releasePreview);
})();
