export const parseApiError = (err: any): string => {
    const data = err?.response?.data;
    if (typeof data === 'string') return data.toUpperCase();
    if (data?.errors) return Object.values(data.errors).flat().join(" | ");
    return data?.title || data?.error || data?.message || "SYSTEM_FAILURE";
};