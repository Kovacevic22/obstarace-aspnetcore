import { useState, useEffect, useCallback } from "react";
import raceService from "../services/raceService.ts";
import obstacleService from "../services/obstacleService.ts";
import { registrationService } from "../services/registrationService.ts";
import type {RaceDto} from "../Models/races.type.ts";
import type {ObstacleDto} from "../Models/obstacles.type.ts";
import type {RegistrationDto} from "../Models/registrations.type.ts";
import {parseApiError} from "../utils/errorParser.ts";

export const useOrganiserData = () => {
    const [races, setRaces] = useState<RaceDto[]>([]);
    const [obstacles, setObstacles] = useState<ObstacleDto[]>([]);
    const [pendingRegistrations, setPendingRegistrations] = useState<RegistrationDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const refreshData = useCallback(async () => {
        try {
            setLoading(true);
            const [racesData, obstaclesData, registrationsData] = await Promise.all([
                raceService.getMyRaces(),
                obstacleService.getObstacles(),
                registrationService.getParticipantsForRace()
            ]);
            setRaces(racesData);
            setObstacles(obstaclesData);
            setPendingRegistrations(registrationsData);
            setError(null);
        } catch (e) {
            setError(parseApiError(e));
        } finally {
            setLoading(false);
        }
    }, []);

    const fetchRegistrationsForRace = async (raceId: number) => {
        try {
            setLoading(true);
            const data = await registrationService.getParticipantsForRace(raceId);
            setPendingRegistrations(data);
        } catch (e) {
            setError(parseApiError(e));
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        void refreshData();
    }, [refreshData]);

    return {
        races,
        obstacles,
        pendingRegistrations,
        loading,
        error,
        refreshData,
        fetchRegistrationsForRace,
        parseApiError
    };
};