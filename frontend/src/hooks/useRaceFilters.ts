import { useSearchParams } from "react-router";
import { useState, useEffect } from "react";
import raceService from "../services/raceService.ts";
import { AxiosError } from "axios";
import type {RaceDto} from "../Models/races.type.ts";

export const useRaceFilters = () => {
    const [races, setRaces] = useState<RaceDto[]>([]);
    const [searchParams, setSearchParams] = useSearchParams();
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");

    const currentSearch = searchParams.get("search") || "";
    const currentDifficulty = searchParams.get("difficulty") || "all";
    const currentDistance = searchParams.get("distanceRange") || "all";

    const filters = {
        search: currentSearch,
        difficulty: currentDifficulty,
        distance: currentDistance
    };

    const updateFilters = (newFilters: any) => {
        const params: any = {};

        if (newFilters.search) {
            params.search = newFilters.search;
        }

        if (newFilters.difficulty && newFilters.difficulty !== "all") {
            params.difficulty = newFilters.difficulty;
        }

        if (newFilters.distance && newFilters.distance !== "all") {
            params.distanceRange = newFilters.distance;
        }

        setSearchParams(params);
    };

    useEffect(() => {
        const fetchData = async () => {
            setLoading(true);
            try {
                const data = await raceService.races(filters);
                setRaces(data);
            } catch (err) {
                if (err instanceof AxiosError) {
                    setError("Error loading races");
                }
            } finally {
                setLoading(false);
            }
        };
        const timer = setTimeout(() => {
            fetchData();
        }, 300);

        return () => clearTimeout(timer);
    }, [searchParams]);
    return {
        races,
        filters,
        updateFilters,
        loading,
        error
    };
};