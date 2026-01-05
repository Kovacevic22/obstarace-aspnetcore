import {useEffect, useState} from "react";
import {type RaceDto, type RaceStatsDto, Status} from "../../Models/races.type.ts";
import raceService from "../../services/raceService.ts";
import userService from "../../services/userService.ts";
import {Role, type UserDto, type UserStatsDto} from "../../Models/users.type.ts";

export function AdminDashboard() {
    const [races, setRaces] = useState<RaceDto[]>([]);
    const [users, setUsers] = useState<UserDto[]>([]);
    const [raceStats, setRaceStats] = useState<RaceStatsDto>();
    const [userStats, setUserStats] = useState<UserStatsDto>();
    const [loading, setLoading] = useState(true);
    const [activeTab, setActiveTab] = useState<'races' | 'users'>('races');


    useEffect(() => {
       const fetchDashboardData = async () =>{
           try{
               setLoading(true);
               const [racesData,usersData,raceStats,userStats] = await Promise.all([
                  raceService.races(),
                   userService.users(),
                   raceService.stats(),
                   userService.stats()
               ]);
               setRaces(racesData);
               setUsers(usersData);
               setRaceStats(raceStats);
               setUserStats(userStats);
           }catch (e){
               console.error(e);
           }finally {
               setLoading(false);
           }
        }
        void fetchDashboardData();
    },[]);

    return (
        <div className="min-h-screen bg-dark text-light p-4 md:p-6 lg:p-12">
            <div className="h-20 md:h-28 w-full" />
            <div className="flex flex-col md:flex-row justify-between items-start md:items-end gap-6 mb-10">
                <div className="space-y-1">
                    <h1 className="text-3xl md:text-5xl font-black uppercase italic tracking-tighter text-white drop-shadow-2xl">
                        Command Center
                    </h1>
                    <div className="flex items-center gap-3">
                        <span className="w-2 h-2 bg-accent animate-pulse rounded-full"></span>
                        <p className="text-[9px] md:text-[10px] uppercase tracking-[0.3em] md:tracking-[0.5em] text-white/30 font-bold italic">
                            System Live // Sector: {activeTab === 'races' ? 'Deployments' : 'Personnel'}
                        </p>
                    </div>
                </div>
                {activeTab === 'races' && (
                    <button className="w-full md:w-auto bg-accent text-dark px-8 py-3 font-black uppercase italic text-xs hover:scale-105 transition-all shadow-[0_0_25px_rgba(166,124,82,0.4)] cursor-pointer active:scale-95">
                        + Create New Race
                    </button>
                )}
            </div>
            <div className="flex gap-6 md:gap-10 mb-12 border-b border-white/5 overflow-x-auto no-scrollbar">
                <button
                    onClick={() => setActiveTab('races')}
                    className={`pb-4 text-[10px] md:text-[11px] font-black uppercase tracking-[0.2em] transition-all cursor-pointer whitespace-nowrap ${activeTab === 'races' ? 'text-accent border-b-2 border-accent' : 'text-white/20 hover:text-white/60'}`}
                >
                    Race Control
                </button>
                <button
                    onClick={() => setActiveTab('users')}
                    className={`pb-4 text-[10px] md:text-[11px] font-black uppercase tracking-[0.2em] transition-all cursor-pointer whitespace-nowrap ${activeTab === 'users' ? 'text-accent border-b-2 border-accent' : 'text-white/20 hover:text-white/60'}`}
                >
                    User Directory
                </button>
            </div>
            <div className="grid grid-cols-2 md:grid-cols-4 gap-4 md:gap-6 mb-12 font-black italic">
                {activeTab === 'races' ? (
                    <>
                        <div className="bg-white/3 border border-white/10 p-4 md:p-8">
                            <span className="text-[8px] md:text-[9px] uppercase tracking-widest text-white/20 block mb-2">Total Races</span>
                            <span className="text-2xl md:text-4xl text-white italic leading-none">{raceStats?.totalRaces || 0}</span>
                        </div>
                        <div className="bg-white/3 border border-white/10 p-4 md:p-8">
                            <span className="text-[8px] md:text-[9px] uppercase tracking-widest text-white/20 block mb-2 font-bold">Total KM</span>
                            <span className="text-2xl md:text-4xl text-accent italic leading-none">{raceStats?.totalKilometers || 0}</span>
                        </div>
                        <div className="bg-white/3 border border-white/10 p-4 md:p-8 opacity-40 col-span-2 md:col-span-1">
                            <span className="text-[8px] md:text-[9px] uppercase tracking-widest text-white/20 block mb-2">Archived</span>
                            <span className="text-2xl md:text-4xl text-white italic leading-none">{raceStats?.archivedCount || 0}</span>
                        </div>
                    </>
                ) : (
                    <>
                        <div className="bg-white/3 border border-white/10 p-4 md:p-8">
                            <span className="text-[8px] md:text-[9px] uppercase tracking-widest text-white/20 block mb-2 font-bold">Total Users</span>
                            <span className="text-2xl md:text-4xl text-white italic leading-none tracking-tighter">{userStats?.totalUsers || 0}</span>
                        </div>
                        <div className="bg-white/3 border border-white/10 p-4 md:p-8 border-l-2 border-l-secondary/50">
                            <span className="text-[8px] md:text-[9px] uppercase tracking-widest block mb-2 font-bold text-secondary/60">Banned Users</span>
                            <span className="text-2xl md:text-4xl text-secondary italic leading-none tracking-tighter">{userStats?.bannedUsers || 0}</span>
                        </div>
                    </>
                )}
            </div>
            <div className="bg-white/2 border border-white/10 overflow-x-auto shadow-2xl no-scrollbar">
                <table className="w-full text-left border-collapse min-w-150 md:min-w-full">
                    <thead className="text-[10px] uppercase tracking-[0.2em] text-white/20 border-b border-white/10 bg-white/1">
                    <tr>
                        <th className="p-4 md:p-6 font-black w-1/3">Designation</th>
                        <th className="p-4 md:p-6 font-black text-center w-1/3">Status / Role</th>
                        <th className="p-4 md:p-6 font-black text-right w-1/3">Operations</th>
                    </tr>
                    </thead>
                    <tbody className="divide-y divide-white/5 uppercase italic font-black">
                    {loading ? (
                        [...Array(5)].map((_, i) => (
                            <tr key={i} className="animate-pulse border-b border-white/5">
                                <td className="p-4 md:p-6"><div className="h-6 bg-white/5 w-48"></div></td>
                                <td className="p-4 md:p-6"><div className="h-6 bg-white/5 w-24 mx-auto"></div></td>
                                <td className="p-4 md:p-6 text-right"><div className="h-6 bg-white/5 w-20 ml-auto"></div></td>
                            </tr>
                        ))
                    ) : activeTab === 'races' ? (
                        races.map((race) => (
                            <tr key={race.id} className="hover:bg-white/3 transition-all group border-l-2 border-l-transparent hover:border-l-accent whitespace-nowrap md:whitespace-normal">
                                <td className="p-4 md:p-6 text-white text-base md:text-lg tracking-tighter">{race.name}</td>
                                <td className="p-4 md:p-6 text-center">
                                    <span className="text-[10px] md:text-[12px] bg-accent/10 text-white px-3 py-1 border border-accent/20">{Status[race.status]}</span>
                                </td>
                                <td className="p-4 md:p-6 text-right">
                                    <button className="text-accent text-[10px] tracking-[0.2em] hover:brightness-125 cursor-pointer">[ EDIT ]</button>
                                </td>
                            </tr>
                        ))
                    ) : (
                        users.map((user) => (
                            <tr key={user.id} className="hover:bg-white/3 transition-all group border-l-2 border-l-transparent hover:border-l-secondary whitespace-nowrap md:whitespace-normal">
                                <td className="p-4 md:p-6">
                                    <div className="text-white text-base md:text-lg tracking-tighter font-black">{user.name} {user.surname}</div>
                                    <div className="text-[9px] md:text-[10px] text-white/20 lowercase font-bold not-italic tracking-normal">{user.email}</div>
                                </td>
                                <td className="p-4 md:p-6 text-center">
                                    <span className="text-[9px] md:text-[10px] text-white/40 tracking-widest font-black uppercase">{Role[user.role] || "Unknown"}</span>
                                </td>
                                <td className="p-4 md:p-6 text-right">
                                    <button
                                        disabled={user.role === 1}
                                        className={`text-[8px] md:text-[9px] tracking-[0.2em] border px-3 md:px-5 py-2 transition-all font-black uppercase italic
                                    ${user.role === 1
                                            ? "opacity-50 border-white/10 text-white cursor-not-allowed bg-white/5"
                                            : "text-secondary/60 border-secondary/20 hover:bg-secondary hover:text-white cursor-pointer active:scale-95 shadow-sm"
                                        }`}>
                                        {user.role === 1 ? (
                                            <span className="flex items-center gap-2 justify-end">
                                        <span className="w-1 h-1 bg-white/40 rounded-full animate-pulse"></span>Protected</span>
                                        ) : "Terminate"}
                                    </button>
                                </td>
                            </tr>
                        ))
                    )}
                    </tbody>
                </table>
            </div>
        </div>
    );
}

export default AdminDashboard;