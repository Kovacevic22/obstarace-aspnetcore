import { useState, useEffect, useCallback } from "react";
import raceService from "../services/raceService.ts";
import userService from "../services/userService.ts";
import organiserService from "../services/organiserService.ts";
import type { RaceDto, RaceStatsDto } from "../Models/races.type.ts";
import type { UserDto, UserStatsDto } from "../Models/users.type.ts";
import type { OrganiserPendingDto } from "../Models/organiser.type.ts";

export const useAdminData = () => {
    const [races, setRaces] = useState<RaceDto[]>([]);
    const [users, setUsers] = useState<UserDto[]>([]);
    const [raceStats, setRaceStats] = useState<RaceStatsDto>();
    const [userStats, setUserStats] = useState<UserStatsDto>();
    const [pendingOrganisers, setPendingOrganisers] = useState<OrganiserPendingDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const parseApiError = (err: any): string => {
        const data = err?.response?.data;
        if (data?.errors) return Object.values(data.errors).flat().join(" | ");
        return data?.title || data?.error || "Error";
    };
    const fetchDashboardData = useCallback(async () => {
        try {
            setLoading(true);
            const [racesData, usersData, rStats, uStats, organisersData] = await Promise.all([
                raceService.races(),
                userService.users(),
                raceService.stats(),
                userService.stats(),
                organiserService.getPending()
            ]);
            setRaces(racesData);
            setUsers(usersData);
            setRaceStats(rStats);
            setUserStats(uStats);
            setPendingOrganisers(organisersData);
            setError(null);
        } catch (e) {
            setError(parseApiError(e));
        } finally {
            setLoading(false);
        }
    }, []);
    const executeAction = async (type: string, userId: number) => {
        try {
            setLoading(true);
            if (type === "approve") await organiserService.verifyOrganiser(userId);
            if (type === "reject") await organiserService.rejectOrganiser(userId);
            if (type === "ban") await userService.banUser(userId);
            if (type === "unban") await userService.unbanUser(userId);
            await fetchDashboardData();
            return true;
        } catch (e) {
            setError(parseApiError(e));
            return false;
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        void fetchDashboardData();
    }, [fetchDashboardData]);

    return {
        races, users, raceStats, userStats, pendingOrganisers,
        loading, error, fetchDashboardData, executeAction, parseApiError
    };
};