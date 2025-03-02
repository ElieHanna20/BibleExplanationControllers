// import React, { lazy, useEffect, useState, useTransition } from "react";
// import { Sidebar } from "flowbite-react";

// const fetchNavigationData = (): Promise<[string, number][]> => {
//   const serverData: [string, number][] = [
//     ["Matthew", 28],
//     ["Mark", 16],
//     ["Luke", 24],
//     ["John", 21],
//     ["Acts", 28],
//     ["Romans", 16],
//     ["1 Corinthians", 16],
//     ["2 Corinthians", 13],
//     ["Galatians", 6],
//     ["Ephesians", 6],
//     ["Philippians", 4],
//     ["Colossians", 4],
//     ["1 Thessalonians", 5],
//     ["2 Thessalonians", 3],
//     ["1 Timothy", 6],
//     ["2 Timothy", 4],
//     ["Titus", 3],
//     ["Philemon", 1],
//     ["Hebrews", 13],
//     ["James", 5],
//     ["1 Peter", 5],
//     ["2 Peter", 3],
//     ["1 John", 5],
//     ["2 John", 1],
//     ["3 John", 1],
//     ["Jude", 1],
//     ["Revelation", 22],
//   ];
//   return new Promise((resolve) => setTimeout(() => resolve(serverData), 1000));
// };

// const NavigationList: React.FC<{ data: [string, number][] }> = ({ data }) => (
//   <Sidebar.ItemGroup>
//     {data.map(([book, chapters], index) => (
//       <Sidebar.Item key={index}>
//         {book} - {chapters} chapters
//       </Sidebar.Item>
//     ))}
//   </Sidebar.ItemGroup>
// );

// const NavigationData = lazy(async () => {
//   let data: [string, number][];
//   if (typeof window !== "undefined") {
//     const savedData = localStorage.getItem("navigationData");
//     if (savedData) {
//       data = JSON.parse(savedData);
//     } else {
//       const [startTransition] = useTransition();
//       data = await startTransition(() => fetchNavigationData());
//       localStorage.setItem("navigationData", JSON.stringify(data));
//     }
//   } else {
//     data = await fetchNavigationData();
//   }
//   return { default: () => <NavigationList data={data} /> };
// });

// export default NavigationData;
