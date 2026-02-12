import { useSearchParams } from "react-router";
import { useState, useEffect, useCallback, useMemo } from "react";
import raceService from "../services/raceService.ts";
import type { RaceDto } from "../Models/races.type.ts";
import { parseApiError } from "../utils/errorParser.ts";

export const useRaceFilters = () => {
    const [races, setRaces] = useState<RaceDto[]>([]);
    const [searchParams, setSearchParams] = useSearchParams();
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");

    const filters = useMemo(() => ({
        search: searchParams.get("search") || "",
        difficulty: searchParams.get("difficulty") || "all",
        distance: searchParams.get("distanceRange") || "all"
    }), [searchParams]);

    const updateFilters = (newFilters: any) => {
        const params: any = {};
        if (newFilters.search) params.search = newFilters.search;
        if (newFilters.difficulty && newFilters.difficulty !== "all") params.difficulty = newFilters.difficulty;
        if (newFilters.distance && newFilters.distance !== "all") params.distanceRange = newFilters.distance;
        setSearchParams(params);
    };
    const fetchData = useCallback(async () => {
        setLoading(true);
        try {
            const data = await raceService.races(filters);
            setRaces(data);
            setError("");
        } catch (err) {
            setError(parseApiError(err));
        } finally {
            setLoading(false);
        }
    }, [filters]);

    useEffect(() => {
        const timer = setTimeout(() => {
            void fetchData();
        }, 300);

        return () => clearTimeout(timer);
    }, [fetchData]);

    return {
        races,
        filters,
        updateFilters,
        loading,
        error
    };
};