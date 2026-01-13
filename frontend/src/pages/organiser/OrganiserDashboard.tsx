import {useCallback, useEffect, useState} from "react";
import {Difficulty, type RaceDto, Status} from "../../Models/races.type.ts";
import raceService from "../../services/raceService.ts";
import CreateRace from "../../components/races/CreateRace.tsx";
import EditRace from "../../components/races/EditRace.tsx";
import type {ObstacleDto} from "../../Models/obstacles.type.ts";
import obstacleService from "../../services/obstacleService.ts";
import type {RegistrationDto} from "../../Models/registrations.type.ts";
import {registrationService} from "../../services/registrationService.ts";
import ConfirmModal from "../../components/common/ConfirmModal.tsx";
import CountRegistrations from "../../components/common/CountRegistrations.tsx";

export function OrganiserDashboard() {
    const [activeTab, setActiveTab] = useState<'races' | 'obstacles' | 'registrations'>('races');
    const [selectedRaceFilter, setSelectedRaceFilter] = useState<number>();
    const [races, setRaces] = useState<RaceDto[]>([]);
    const [obstacles, setObstacles] = useState<ObstacleDto[]>([]);
    const [pendingRegistrations, setPendingRegistrations] = useState<RegistrationDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string|null>();
    const [modalError, setModalError] = useState<string|null>(null);
    const [raceModal, setRaceModal] = useState<{
        isOpen: boolean;
        mode: 'create' | 'edit' | null;
        id: number | null;
    }>({ isOpen: false, mode: null, id: null });
    const [confirmConfig, setConfirmConfig] = useState<{
        isOpen: boolean;
        type: 'confirm' | 'cancel'| 'delete' | null;
        id: number | null;
    }>({ isOpen: false, type: null, id: null});
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
        } catch (e) {
            console.error(e);
            setError(parseApiError(e));
        } finally {
            setLoading(false);
        }
    },[]);

    useEffect(() => {
        void refreshData();
    }, [refreshData]);


    const openRaceModal = (mode: 'create' | 'edit', id: number | null = null) => {
        setRaceModal({ isOpen: true, mode, id });
    };

    const closeRaceModal = () => {
        setRaceModal({ isOpen: false, mode: null, id: null });
    };
    ////FUNCTIONS
    const parseApiError = (error : any): string => {
        const data = error?.response?.data;

        if (data?.errors) {
            return Object.values(data.errors).flat().join(" | ");
        }

        return data?.title || data?.error || "Error";
    };
    ////////////////////////////

    const handleRaceSelect = async (raceId: number)=> {
        setSelectedRaceFilter(raceId);
        try {
            setLoading(true);
            const data = await registrationService.getParticipantsForRace(raceId);
            setPendingRegistrations(data);
        } catch (e) {
            console.error(e);
        } finally {
            setLoading(false);
        }
    }
    const handleConfirmation = async ()=>{
        if(!confirmConfig.id || !confirmConfig.type)return;
        try{
            setLoading(true);
            setModalError(null);
            if(confirmConfig.type == 'confirm'){
                await registrationService.confirmUserRegistration(confirmConfig.id);
            }
            if(confirmConfig.type == 'cancel'){
                await registrationService.cancelUserRegistration(confirmConfig.id);
            }
            if(confirmConfig.type == 'delete'){
                await obstacleService.removeObstacle(confirmConfig.id);
            }
            setConfirmConfig(prev=>({...prev, isOpen:false}));
            void refreshData();
        }catch(err){
            console.error(err);
            setModalError(parseApiError(err));
        }finally {
            setLoading(false);
        }
    }
    if (loading) {
        return (
            <div className="min-h-screen bg-dark flex items-center justify-center">
                <div className="flex flex-col items-center gap-4">
                    <div className="w-12 h-12 border-2 border-accent border-t-transparent animate-spin" />
                    <p className="text-[10px] font-black uppercase tracking-[0.5em] text-accent animate-pulse italic">Synchronizing_Data...</p>
                </div>
            </div>
        );
    }

    if (error) {
        return (
            <div className="min-h-screen bg-dark flex items-center justify-center p-6">
                <div className="max-w-md w-full bg-red-500/10 border border-red-500/50 p-8 backdrop-blur-xl">
                    <div className="flex items-start gap-4">
                        <div className="w-2 h-2 bg-red-500 mt-1 animate-pulse" />
                        <div className="space-y-4">
                            <h3 className="text-[10px] font-black uppercase tracking-widest text-red-500">System_Failure // Error</h3>
                            <p className="text-xs font-bold text-white/80 leading-relaxed uppercase">{error}</p>
                            <button
                                onClick={() => window.location.reload()}
                                className="text-[10px] font-black uppercase tracking-widest text-white hover:text-accent transition-colors cursor-pointer block"
                            >
                                [ Reboot_System ]
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        );
    }

    return (
        <div className="min-h-screen bg-dark text-light p-4 md:p-6 lg:p-12">
            <div className="h-20 md:h-28 w-full" />


            <div className="flex flex-col md:flex-row justify-between items-start md:items-end gap-6 mb-10">
                <div className="space-y-1">
                    <h1 className="text-3xl md:text-5xl font-black uppercase italic tracking-tighter text-white drop-shadow-2xl">
                        Organiser <span className="text-accent">Dashboard</span>
                    </h1>
                    <div className="flex items-center gap-3">
                        <span className="w-2 h-2 bg-accent animate-pulse rounded-full"></span>
                        <p className="text-[9px] md:text-[10px] uppercase tracking-[0.3em] text-white/30 font-bold italic">
                            Unit Ops // Sector: {activeTab.toUpperCase()}
                        </p>
                    </div>
                </div>

                <div className="flex gap-4 w-full md:w-auto">
                    <button
                        onClick={()=>openRaceModal("create")}
                        className="flex-1 md:flex-none bg-accent text-dark px-8 py-3 font-black uppercase italic text-xs hover:scale-105 transition-all shadow-[0_0_25px_rgba(166,124,82,0.4)] cursor-pointer active:scale-95">
                        + New Race
                    </button>
                </div>
            </div>


            <div className="flex gap-6 md:gap-10 mb-12 border-b border-white/5 overflow-x-auto no-scrollbar">
                {['races', 'obstacles', 'registrations'].map((tab) => (
                    <button
                        key={tab}
                        onClick={() => setActiveTab(tab as any)}
                        className={`pb-4 text-[10px] md:text-[11px] font-black uppercase tracking-[0.2em] transition-all cursor-pointer whitespace-nowrap ${
                            activeTab === tab ? 'text-accent border-b-2 border-accent' : 'text-white/20 hover:text-white/60'
                        }`}
                    >
                        {tab === 'registrations' ? 'Personnel Clearances' : `My ${tab}`}
                    </button>
                ))}
            </div>


            {activeTab === 'registrations' && (
                <div className="mb-8 flex flex-col md:flex-row items-center gap-4 bg-white/3 p-4 border border-white/5">
                    <label className="text-[10px] font-black uppercase tracking-widest text-accent italic">Select Race:</label>
                    <select
                        value={selectedRaceFilter}
                        onChange={(e) => handleRaceSelect(Number(e.target.value))}
                        className="bg-dark border border-white/10 text-white text-[10px] font-black uppercase px-4 py-2 outline-none focus:border-accent transition-all cursor-pointer"
                    >
                        <option value={0}>-- ALL ACTIVE MISSIONS --</option>
                        {races.map(race => (
                            <option key={race.id} value={race.id}>
                                {race.name}
                            </option>
                        ))}
                    </select>
                </div>
            )}


            <div className="bg-white/2 border border-white/10 overflow-x-auto shadow-2xl no-scrollbar">
                <table className="w-full text-left border-collapse min-w-150 md:min-w-full">
                    <thead className="text-[10px] uppercase tracking-[0.2em] text-white/20 border-b border-white/10 bg-white/1">
                    <tr>
                        <th className="p-4 md:p-6 font-black w-1/3 italic">Designation</th>
                        <th className="p-4 md:p-6 font-black text-center w-1/4 italic">Participants</th>
                        <th className="p-4 md:p-6 font-black text-center w-1/3 italic">Status / Class</th>
                        <th className="p-4 md:p-6 font-black text-right w-1/3 italic">Operations</th>
                    </tr>
                    </thead>
                    <tbody className="divide-y divide-white/5 uppercase italic font-black">

                    {activeTab === 'races' && (
                        races.length > 0 ? (
                            races.map((race: RaceDto) => (
                                <tr key={race.id} className="hover:bg-white/3 transition-all group border-l-2 border-l-transparent hover:border-l-accent whitespace-nowrap md:whitespace-normal">
                                    <td className="p-4 md:p-6">
                                        <div className="text-white text-base md:text-lg tracking-tighter font-black uppercase italic">
                                            {race.name}
                                        </div>
                                        <div className="text-[9px] text-white/20 not-italic font-bold tracking-widest uppercase">
                                            Sector: {race.location} // {race.distance}KM
                                        </div>
                                    </td>
                                    <td className="p-4 md:p-6 text-center">
                                        <div className="text-white text-base md:text-lg tracking-tighter font-black uppercase italic">
                                            <CountRegistrations raceId={race.id}/> <span className="text-accent/40">/</span> {race.maxParticipants}
                                        </div>
                                        <div className="text-[9px] text-white/20 not-italic font-bold tracking-widest uppercase">
                                            Registered Units
                                        </div>
                                    </td>
                                    <td className="p-4 md:p-6 text-center">
                    <span className={`text-[9px] md:text-[10px] px-3 py-1 border font-black ${
                        race.status === 0 ? "bg-accent/10 text-white border-accent/20" : "bg-white/5 text-white/40 border-white/10"
                    }`}>
                        {Status[race.status]}
                    </span>
                                    </td>
                                    <td className="p-4 md:p-6 text-right">
                                        <button
                                            onClick={() => openRaceModal('edit',race.id)}
                                            className="text-accent text-[10px] tracking-[0.2em] hover:brightness-125 cursor-pointer font-black"
                                        >
                                            [ MODIFY ]
                                        </button>
                                    </td>
                                </tr>
                            ))
                        ) : (
                            <tr>
                                <td colSpan={3} className="p-20 text-center text-white/10 tracking-[1em]">NO_RACES</td>
                            </tr>
                        )
                    )}


                    {activeTab === 'obstacles' && (
                        obstacles.length > 0 ? (
                            obstacles.map((obstacle: ObstacleDto) => (
                                <tr key={obstacle.id} className="hover:bg-white/3 transition-all group border-l-2 border-l-transparent hover:border-l-white">
                                    <td className="p-4 md:p-6">
                                        <div className="text-white text-base md:text-lg tracking-tighter uppercase font-black">
                                            {obstacle.name}
                                        </div>
                                        <div className="text-[9px] text-white/30 not-italic font-bold tracking-widest uppercase mt-1 max-w-xs truncate md:max-w-md">
                                            Description: {obstacle.description || "NO_SPECIFICATIONS_PROVIDED"}
                                        </div>
                                    </td>
                                    <td className="p-4 md:p-6 text-center">
                                                    <span className={`px-4 py-2 border text-[11px] font-black tracking-widest uppercase italic ${
                                                        obstacle.difficulty === 2 ? 'bg-red-500/10 text-red-500 border-red-500/20' :
                                                            obstacle.difficulty === 1 ? 'bg-orange-500/10 text-orange-500 border-orange-500/20' :
                                                                'bg-green-500/10 text-green-500 border-green-500/20'
                                                    }`}>
                                                        {Difficulty[obstacle.difficulty]}
                                                    </span>
                                    </td>
                                    <td className="p-4 md:p-6 text-right">
                                        <button
                                            onClick={()=>setConfirmConfig({isOpen:true,type:'delete',id:obstacle.id})}
                                            className="text-white/40 text-[10px] tracking-[0.2em] hover:text-red-500 cursor-pointer transition-colors font-black"
                                        >
                                            [ DELETE ]
                                        </button>
                                    </td>
                                </tr>
                            ))
                        ) : (
                            <tr>
                                <td colSpan={3} className="p-20 text-center text-white/10 tracking-[1em]">NO_OBSTACLES</td>
                            </tr>
                        )
                    )}

                    {activeTab === 'registrations' && (
                        pendingRegistrations.length > 0 ? (
                            pendingRegistrations.map((registration: RegistrationDto) => (
                                <tr className="hover:bg-white/3 transition-all group border-l-2 border-l-transparent hover:border-l-accent">
                                    <td className="p-4 md:p-6">
                                        <div className="text-white text-base md:text-lg tracking-tighter font-black">
                                            {registration.participant.name} {registration.participant.surname}
                                        </div>
                                        <div className="text-[9px] md:text-[10px] text-accent font-bold not-italic tracking-widest uppercase">Bib: #{registration.bibNumber} (Auto-Gen)</div>
                                    </td>
                                    <td className="p-4 md:p-6 text-center">
                                        <span className="text-[9px] md:text-[10px] bg-white/5 text-white/40 px-3 py-1 border border-white/10 font-black">PENDING_STATUS</span>
                                    </td>
                                    <td className="p-4 md:p-6 text-right space-x-3">
                                        <button
                                            onClick={()=>setConfirmConfig({isOpen:true,type:'confirm',id:registration.id})}
                                            className="text-[8px] md:text-[9px] border border-green-500/20 text-green-500 px-4 py-2 hover:bg-green-500 hover:text-white transition-all cursor-pointer font-black">[ CONFIRM ]</button>
                                        <button
                                            onClick={()=>setConfirmConfig({isOpen:true,type:'cancel',id:registration.id})}
                                            className="text-[8px] md:text-[9px] border border-red-500/20 text-red-500 px-4 py-2 hover:bg-red-500 hover:text-white transition-all cursor-pointer font-black">[ CANCEL ]</button>
                                    </td>
                                </tr>
                            ))):(
                            <tr>
                                <td colSpan={3} className="p-20 text-center text-white/10 tracking-[1em]">NO_REGISTRATIONS</td>
                            </tr>
                        )
                    )}
                    </tbody>
                </table>
            </div>

            <div className="mt-8 flex justify-between items-center opacity-20 border-t border-white/10 pt-4">
                <div className="text-[8px] uppercase tracking-[0.5em] font-black">Auth_Level: Organiser // Grid: Stable</div>
                <div className="text-[8px] uppercase tracking-[0.5em] font-black italic underline decoration-accent/30 underline-offset-4">LOGGING_ACTIVE</div>
            </div>
            {raceModal.isOpen && (
                raceModal.mode === 'create' ? (
                    <CreateRace
                        isOpen={raceModal.isOpen}
                        onClose={closeRaceModal}
                        onSuccess={refreshData}
                    />
                ) : (
                    <EditRace
                        id={raceModal.id || 0}
                        isOpen={raceModal.isOpen}
                        onClose={closeRaceModal}
                        onSuccess={refreshData}
                    />
                )
            )}
            <ConfirmModal
                isOpen={confirmConfig.isOpen}
                onClose={() => setConfirmConfig(prev => ({ ...prev, isOpen: false }))}
                onConfirm={handleConfirmation}
                title={
                    confirmConfig.type === 'confirm' ? "AUTHORIZE" :
                        confirmConfig.type === 'cancel' ? "REJECT" : "TERMINATE OBSTACLE"
                }
                variant={confirmConfig.type === 'confirm' ? "success" : "danger"}
                message={
                    confirmConfig.type === 'delete'
                        ? "ARE YOU SURE YOU WANT TO PERMANENTLY REMOVE THIS OBSTACLE FROM THE DATABASE?"
                        : `ARE YOU SURE YOU WANT TO ${confirmConfig.type?.toUpperCase()} THIS REGISTRATION?`
                }
                error={modalError}
            />
        </div>
    );
}
export default OrganiserDashboard