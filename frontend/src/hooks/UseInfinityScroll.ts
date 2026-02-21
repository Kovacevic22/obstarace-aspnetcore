import { useState, useEffect, useRef, useCallback } from "react";

export function useInfiniteScroll<T>(
    fetchFn: (page: number) => Promise<T[]>,
    deps: unknown[] = []
) {
    const [items, setItems] = useState<T[]>([]);
    const [loading, setLoading] = useState(true);
    const [hasMore, setHasMore] = useState(true);
    const sentinelRef = useRef<HTMLDivElement>(null);
    const fetching = useRef(false);
    const currentPage = useRef(1);

    const load = useCallback(async (p: number, reset: boolean) => {
        if (fetching.current) return;
        fetching.current = true;
        if (reset) setLoading(true);
        try {
            const data = await fetchFn(p);
            setItems(prev => reset ? data : [...prev, ...data]);
            setHasMore(data.length === 12);
        } finally {
            if (reset) setLoading(false);
            fetching.current = false;
        }
    }, [fetchFn]);

    useEffect(() => {
        currentPage.current = 1;
        setHasMore(true);
        load(1, true);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, deps);

    useEffect(() => {
        if (!hasMore || loading) return;
        const observer = new IntersectionObserver(([entry]) => {
            if (entry.isIntersecting) {
                currentPage.current += 1;
                load(currentPage.current, false);
            }
        });
        if (sentinelRef.current) observer.observe(sentinelRef.current);
        return () => observer.disconnect();
    }, [hasMore, loading, load]);

    return { items, loading, hasMore, sentinelRef };
}