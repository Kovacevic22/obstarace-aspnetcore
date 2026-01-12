import {registrationService} from "../../services/registrationService.ts";
import {useEffect, useState} from "react";

function ParticipantCount({ raceId }: { raceId: number }) {
    const [count, setCount] = useState<number>(0);

    useEffect(() => {
        const fetchCount = async () => {
            try {
                const data = await registrationService.countRegistrations(raceId);
                setCount(data);
            } catch (e) {
                console.error(e);
            }
        };
        void fetchCount();
    }, [raceId]);

    return <>{count}</>;
}
export default ParticipantCount;