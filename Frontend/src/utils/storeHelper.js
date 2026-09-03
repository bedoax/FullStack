export async function execute(loading, action) {
    loading.value = true;

    try {
        return await action();
    }
    finally {
        loading.value = false;
    }
}