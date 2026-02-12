import { useState, useEffect, useCallback } from "react";
import raceService from "../services/raceService.ts";
import { registrationService } from "../services/registrationService.ts";
import { useNavigate } from "react-router";
import type {RaceDto} from "../Models/races.type.ts";
import {parseApiError} from "../utils/errorParser.ts";

export const useRaceDetails = (slug: string | undefined, userId: number | undefined) => {
    const navigate = useNavigate();
    const [race, setRace] = useState<RaceDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [isRegistering, setIsRegistering] = useState(false);
    const [isAlreadyRegistered, setIsAlreadyRegistered] = useState(false);
    const fetchRace = useCallback(async () => {
        if (!slug) return;
        try {
            setLoading(true);
            const data = await raceService.raceDetails(slug);
            setRace(data);
            if (userId && data.id) {
                const registered = await registrationService.isUserRegistered(data.id);
                setIsAlreadyRegistered(registered);
            }
            setError(null);
        } catch (e) {
            setError(parseApiError(e));
        } finally {
            setLoading(false);
        }
    }, [slug, userId]);
    const registerToRace = async () => {
        if (!userId) {
            setError("AUTHENTICATION_REQUIRED: YOU MUST BE LOGGED IN TO REGISTER.");
            return false;
        }
        if (!race) return false;

        try {
            setIsRegistering(true);
            setError(null);
            await registrationService.createRegistration(race.id);
            navigate('/my-registrations');
            return true;
        } catch (e) {
            setError(parseApiError(e));
            return false;
        } finally {
            setIsRegistering(false);
        }
    };

    useEffect(() => {
        void fetchRace();
    }, [fetchRace]);
    const getDaysRemaining = (deadline: string) => {
        const today = new Date();
        const target = new Date(deadline);
        const diffInMs = target.getTime() - today.getTime();
        const diffInDays = Math.ceil(diffInMs / (1000 * 60 * 60 * 24));
        if (diffInDays < 0) return "Completed :(";
        if (diffInDays === 0) return "Ongoing!";
        return diffInDays === 1 ? "1 day left!" : `${diffInDays} days left`;
    };

    const isClosed = race ? new Date(race.registrationDeadLine).getTime() < new Date().getTime() : true;

    return {
        race,
        loading,
        error,
        setError,
        isRegistering,
        isClosed,
        registerToRace,
        getDaysRemaining,
        isAlreadyRegistered
    };
};